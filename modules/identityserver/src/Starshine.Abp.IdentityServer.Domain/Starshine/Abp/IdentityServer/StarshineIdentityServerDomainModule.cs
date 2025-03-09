using Starshine.IdentityServer.Configuration;
using Starshine.IdentityServer.Services;
using Starshine.IdentityServer.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Starshine.Abp.IdentityServer.Tokens;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Security.Claims;
using Volo.Abp.Validation;
using Volo.Abp.Threading;
using Starshine.Abp.Identity;
using Volo.Abp;
using Starshine.Abp.IdentityServer.Events;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer;

/// <summary>
/// IdentityServer模块
/// </summary>
[DependsOn(
    typeof(StarshineIdentityServerDomainSharedModule),
    typeof(StarshineIdentityDomainModule),
    typeof(AbpSecurityModule),
    typeof(AbpCachingModule),
    typeof(AbpValidationModule),
    typeof(AbpBackgroundWorkersModule)
    )]
public class StarshineIdentityServerDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<ApiResource, ApiResourceEto>(typeof(StarshineIdentityServerDomainModule));
            options.EtoMappings.Add<Client, ClientEto>(typeof(StarshineIdentityServerDomainModule));
            options.EtoMappings.Add<DeviceFlowCodes, DeviceFlowCodesEto>(typeof(StarshineIdentityServerDomainModule));
            options.EtoMappings.Add<IdentityResource, IdentityResourceEto>(typeof(StarshineIdentityServerDomainModule));
        });

        Configure<StarshineClaimsServiceOptions>(options =>
        {
            options.RequestedClaims.AddRange(new[]
            {
                AbpClaimTypes.TenantId,
                AbpClaimTypes.EditionId
            });
        });

        AddIdentityServer(context.Services);
    }

    private static void AddIdentityServer(IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        var builderOptions = services.ExecutePreConfiguredActions<StarshineIdentityServerBuilderOptions>();

        var identityServerBuilder = AddIdentityServer(services, builderOptions);

        if (builderOptions.AddDeveloperSigningCredential)
        {
            identityServerBuilder = identityServerBuilder.AddDeveloperSigningCredential();
        }

        identityServerBuilder.AddStarshineIdentityServer(builderOptions);

        services.ExecutePreConfiguredActions(identityServerBuilder);

        if (!services.IsAdded<IPersistedGrantService>())
        {
            services.TryAddSingleton<IPersistedGrantStore, InMemoryPersistedGrantStore>();
        }

        if (!services.IsAdded<IDeviceFlowStore>())
        {
            services.TryAddSingleton<IDeviceFlowStore, InMemoryDeviceFlowStore>();
        }

        if (!services.IsAdded<IClientStore>())
        {
            identityServerBuilder.AddInMemoryClients(configuration.GetSection("IdentityServer:Clients"));
        }

        if (!services.IsAdded<IResourceStore>())
        {
            identityServerBuilder.AddInMemoryApiResources(configuration.GetSection("IdentityServer:ApiResources"));
            identityServerBuilder.AddInMemoryIdentityResources(configuration.GetSection("IdentityServer:IdentityResources"));
        }
    }

    private static IIdentityServerBuilder AddIdentityServer(IServiceCollection services, StarshineIdentityServerBuilderOptions abpIdentityServerBuilderOptions)
    {
        services.Configure<IdentityServerOptions>(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
        });

        var identityServerBuilder = services.AddIdentityServerBuilder()
            .AddRequiredPlatformServices()
            .AddCoreServices()
            .AddDefaultEndpoints()
            .AddPluggableServices()
            .AddValidators()
            .AddResponseGenerators()
            .AddDefaultSecretParsers()
            .AddDefaultSecretValidators();

        if (abpIdentityServerBuilderOptions.AddIdentityServerCookieAuthentication)
        {
            identityServerBuilder.AddCookieAuthentication();
        }

        // provide default in-memory implementation, not suitable for most production scenarios
        identityServerBuilder.AddInMemoryPersistedGrants();

        return identityServerBuilder;
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityServerModuleExtensionConsts.ModuleName,
                IdentityServerModuleExtensionConsts.EntityNames.Client,
                typeof(Client)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityServerModuleExtensionConsts.ModuleName,
                IdentityServerModuleExtensionConsts.EntityNames.IdentityResource,
                typeof(IdentityResource)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityServerModuleExtensionConsts.ModuleName,
                IdentityServerModuleExtensionConsts.EntityNames.ApiResource,
                typeof(ApiResource)
            );
        });
    }

    /// <summary>
    /// 应用程序初始化
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {

        var options = context.ServiceProvider.GetRequiredService<IOptions<TokenCleanupOptions>>().Value;
        if (options.IsCleanupEnabled)
        {
            //await context.ServiceProvider
            //    .GetRequiredService<IBackgroundWorkerManager>()
            //    .AddAsync(
            //        context.ServiceProvider
            //            .GetRequiredService<TokenCleanupBackgroundWorker>()
            //    );
        }
    }

    /// <summary>
    /// 应用程序初始化
    /// </summary>
    /// <param name="context"></param>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }
}
