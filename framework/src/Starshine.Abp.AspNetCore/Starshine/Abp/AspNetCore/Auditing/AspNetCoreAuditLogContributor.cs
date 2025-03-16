// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Logging.Abstractions;
using Starshine.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.AspNetCore.Auditing;

public class AspNetCoreAuditLogContributor(ILogger<AspNetCoreAuditLogContributor> logger) : AuditLogContributor, ITransientDependency
{
    private readonly ILogger _logger = logger;

    public override void PreContribute(AuditLogContributionContext context)
    {
        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        if (httpContext == null)
        {
            return;
        }

        if (httpContext.WebSockets.IsWebSocketRequest)
        {
            return;
        }

        if (context.AuditInfo.HttpMethod == null)
        {
            context.AuditInfo.HttpMethod = httpContext.Request.Method;
        }

        if (context.AuditInfo.Url == null)
        {
            context.AuditInfo.Url = BuildUrl(httpContext);
        }

        var clientInfoProvider = context.ServiceProvider.GetRequiredService<IWebClientInfoProvider>();
        if (context.AuditInfo.ClientIpAddress == null)
        {
            context.AuditInfo.ClientIpAddress = clientInfoProvider.ClientIpAddress;
        }

        if (context.AuditInfo.BrowserInfo == null)
        {
            context.AuditInfo.BrowserInfo = clientInfoProvider.BrowserInfo;
        }

        //TODO: context.AuditInfo.ClientName
    }

    public override void PostContribute(AuditLogContributionContext context)
    {
        if (context.AuditInfo.HttpStatusCode != null)
        {
            return;
        }

        var httpContext = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        if (httpContext == null)
        {
            return;
        }

        if (context.AuditInfo.Exceptions.Count != 0)
        {
            var httpExceptionStatusCodeFinder = context.ServiceProvider.GetRequiredService<IHttpExceptionStatusCodeFinder>();
            foreach (var auditInfoException in context.AuditInfo.Exceptions)
            {
                var statusCode = httpExceptionStatusCodeFinder.GetStatusCode(httpContext, auditInfoException);
                context.AuditInfo.HttpStatusCode = (int)statusCode;
            }

            if (context.AuditInfo.HttpStatusCode != null)
            {
                return;
            }
        }

        context.AuditInfo.HttpStatusCode = httpContext.Response.StatusCode;
    }

    protected virtual string BuildUrl(HttpContext httpContext)
    {
        //TODO: Add options to include/exclude query, schema and host

        var uriBuilder = new UriBuilder
        {
            Scheme = httpContext.Request.Scheme,
            Host = httpContext.Request.Host.Host,
            Path = httpContext.Request.Path.ToString(),
            Query = httpContext.Request.QueryString.ToString()
        };

        return uriBuilder.Uri.AbsolutePath;
    }
}
