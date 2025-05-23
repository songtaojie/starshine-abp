﻿using System.Globalization;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Volo.Abp;
using Volo.Abp.Http;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.AspNetCore.MultiTenancy;

public class AspNetCoreMultiTenancyOptions
{
    /// <summary>
    /// Default: <see cref="TenantResolverConsts.DefaultTenantKey"/>.
    /// </summary>
    public string TenantKey { get; set; }

    /// <summary>
    /// Return true to stop the pipeline, false to continue.
    /// </summary>
    public Func<HttpContext, Exception, Task<bool>> MultiTenancyMiddlewareErrorPageBuilder { get; set; }

    public AspNetCoreMultiTenancyOptions()
    {
        TenantKey = TenantResolverConsts.DefaultTenantKey;
        MultiTenancyMiddlewareErrorPageBuilder = async (context, exception) =>
        {
            var isCookieAuthentication = false;
            var tenantResolveResult = context.RequestServices.GetRequiredService<ITenantResolveResultAccessor>().Result;
            if (tenantResolveResult != null)
            {
                if (tenantResolveResult.AppliedResolvers.Count == 1 && tenantResolveResult.AppliedResolvers.Contains(CurrentUserTenantResolveContributor.ContributorName))
                {
                    var authenticationType = context.User.Identity?.AuthenticationType;
                    if (authenticationType != null)
                    {
                        var scheme = await context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>().GetHandlerAsync(context, authenticationType);
                        if (scheme is CookieAuthenticationHandler cookieAuthenticationHandler)
                        {
                            // Try to delete the authentication's cookie if it does not exist or is inactive.
                            await cookieAuthenticationHandler.SignOutAsync(null);
                            isCookieAuthentication = true;
                        }
                    }
                }

                var options = context.RequestServices.GetRequiredService<IOptions<AspNetCoreMultiTenancyOptions>>().Value;
                if (tenantResolveResult.AppliedResolvers.Contains(CookieTenantResolveContributor.ContributorName) ||
                    context.Request.Cookies.ContainsKey(options.TenantKey))
                {
                    // Try to delete the tenant's cookie if it does not exist or is inactive.
                    AspNetCoreMultiTenancyCookieHelper.SetTenantCookie(context, null, options.TenantKey);
                }
            }

            context.Response.Headers.Append("Abp-Tenant-Resolve-Error", HtmlEncoder.Default.Encode(exception.Message));
            if (isCookieAuthentication && context.Request.Method.Equals("Get", StringComparison.OrdinalIgnoreCase) && !context.Request.IsAjax())
            {
                context.Response.Redirect(context.Request.GetEncodedUrl());
            }
            else if (context.Request.IsAjax())
            {
                var error = new RemoteServiceErrorResponse(new RemoteServiceErrorInfo(exception.Message, exception is BusinessException businessException ? businessException.Details : string.Empty));

                var jsonSerializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;

                ResponseContentTypeHelper.ResolveContentTypeAndEncoding(
                    null,
                    context.Response.ContentType,
                    (new MediaTypeHeaderValue("application/json")
                    {
                        Encoding = Encoding.UTF8
                    }.ToString(), Encoding.UTF8),
                    MediaType.GetEncoding,
                    out var resolvedContentType,
                    out var resolvedContentTypeEncoding);

                context.Response.ContentType = resolvedContentType;
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var responseStream = context.Response.Body;
                if (resolvedContentTypeEncoding.CodePage == Encoding.UTF8.CodePage)
                {
                    try
                    {
                        await JsonSerializer.SerializeAsync(responseStream, error, error.GetType(), jsonSerializerOptions, context.RequestAborted);
                        await responseStream.FlushAsync(context.RequestAborted);
                    }
                    catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested) { }
                }
                else
                {
                    var transcodingStream = Encoding.CreateTranscodingStream(context.Response.Body, resolvedContentTypeEncoding, Encoding.UTF8, leaveOpen: true);
                    ExceptionDispatchInfo? exceptionDispatchInfo = null;
                    try
                    {
                        await JsonSerializer.SerializeAsync(transcodingStream, error, error.GetType(), jsonSerializerOptions, context.RequestAborted);
                        await transcodingStream.FlushAsync(context.RequestAborted);
                    }
                    catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested) { }
                    catch (Exception ex)
                    {
                        exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);
                    }
                    finally
                    {
                        try
                        {
                            await transcodingStream.DisposeAsync();
                        }
                        catch when (exceptionDispatchInfo != null)
                        {
                        }
                        exceptionDispatchInfo?.Throw();
                    }
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "text/html";

                var message = exception.Message;
                var details = exception is BusinessException businessException ? businessException.Details : string.Empty;

                await context.Response.WriteAsync($"<html lang=\"{HtmlEncoder.Default.Encode(CultureInfo.CurrentCulture.Name)}\"><body>\r\n");
                await context.Response.WriteAsync($"<h3>{HtmlEncoder.Default.Encode(message)}</h3>{HtmlEncoder.Default.Encode(details!)}<br>\r\n");
                await context.Response.WriteAsync("</body></html>\r\n");

                // Note the 500 spaces are to work around an IE 'feature'
                await context.Response.WriteAsync(new string(' ', 500));
            }

            return true;
        };
    }
}
