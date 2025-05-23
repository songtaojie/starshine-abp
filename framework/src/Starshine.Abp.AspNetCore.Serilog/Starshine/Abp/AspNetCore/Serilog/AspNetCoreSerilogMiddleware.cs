using Microsoft.Extensions.Options;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Starshine.Abp.AspNetCore.Middleware;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Tracing;
using Volo.Abp.Users;

namespace Starshine.Abp.AspNetCore.Serilog;

public class AspNetCoreSerilogMiddleware : MiddlewareBase, ITransientDependency
{
    private readonly ICurrentClient _currentClient;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly ICorrelationIdProvider _correlationIdProvider;
    private readonly AspNetCoreSerilogOptions _options;

    public AspNetCoreSerilogMiddleware(
        ICurrentTenant currentTenant,
        ICurrentUser currentUser,
        ICurrentClient currentClient,
        ICorrelationIdProvider correlationIdProvider,
        IOptions<AspNetCoreSerilogOptions> options)
    {
        _currentTenant = currentTenant;
        _currentUser = currentUser;
        _currentClient = currentClient;
        _correlationIdProvider = correlationIdProvider;
        _options = options.Value;
    }

    public async override Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var enrichers = new List<ILogEventEnricher>();

        if (_currentTenant?.Id != null)
        {
            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.TenantId, _currentTenant.Id));
        }

        if (_currentUser?.Id != null)
        {
            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.UserId, _currentUser.Id));
        }

        if (_currentClient?.Id != null)
        {
            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.ClientId, _currentClient.Id));
        }

        var correlationId = _correlationIdProvider.Get();
        if (!string.IsNullOrEmpty(correlationId))
        {
            enrichers.Add(new PropertyEnricher(_options.EnricherPropertyNames.CorrelationId, correlationId));
        }

        using (LogContext.Push(enrichers.ToArray()))
        {
            await next(context);
        }
    }
}
