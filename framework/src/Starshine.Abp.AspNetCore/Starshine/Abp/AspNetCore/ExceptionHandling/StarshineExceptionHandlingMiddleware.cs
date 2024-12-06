﻿// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace Starshine.Abp.AspNetCore.ExceptionHandling;

public class AbpExceptionHandlingMiddleware : Volo.Abp.AspNetCore.Middleware.AbpMiddlewareBase, ITransientDependency
{
    private readonly ILogger<AbpExceptionHandlingMiddleware> _logger;

    private readonly Func<object, Task> _clearCacheHeadersDelegate;

    public AbpExceptionHandlingMiddleware(ILogger<AbpExceptionHandlingMiddleware> logger)
    {
        _logger = logger;

        _clearCacheHeadersDelegate = ClearCacheHeaders;
    }

    public async override Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("An exception occurred, but response has already started!");
                throw;
            }

            await HandleAndWrapException(context, ex);
            return;
        }
    }

    private async Task HandleAndWrapException(HttpContext httpContext, Exception exception)
    {
        _logger.LogException(exception);

        await httpContext.RequestServices
            .GetRequiredService<IExceptionNotifier>()
            .NotifyAsync(
                new ExceptionNotificationContext(exception)
            );

        if (exception is AbpAuthorizationException)
        {
            await httpContext.RequestServices.GetRequiredService<IAbpAuthorizationExceptionHandler>()
                .HandleAsync(exception.As<AbpAuthorizationException>(), httpContext);
        }
        else
        {
            var errorInfoConverter = httpContext.RequestServices.GetRequiredService<IExceptionToErrorInfoConverter>();
            var statusCodeFinder = httpContext.RequestServices.GetRequiredService<IHttpExceptionStatusCodeFinder>();
            var jsonSerializer = httpContext.RequestServices.GetRequiredService<IJsonSerializer>();
            var exceptionHandlingOptions = httpContext.RequestServices.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)statusCodeFinder.GetStatusCode(httpContext, exception);
            httpContext.Response.OnStarting(_clearCacheHeadersDelegate, httpContext.Response);
            httpContext.Response.Headers.Append("Content-Type", "application/json");

            await httpContext.Response.WriteAsync(
                jsonSerializer.Serialize(
                    new RemoteServiceErrorResponse(
                        errorInfoConverter.Convert(exception, options =>
                        {
                            options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
                            options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
                        })
                    )
                )
            );
        }
    }

    private Task ClearCacheHeaders(object state)
    {
        var response = (HttpResponse)state;
        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);

        return Task.CompletedTask;
    }
}
