using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.ApiResources;
using Starshine.Abp.IdentityServer.ApiScopes;
using Starshine.Abp.IdentityServer.Clients;
using Starshine.Abp.IdentityServer.Devices;
using Starshine.Abp.IdentityServer.Grants;
using Starshine.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Modularity;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore;

[DependsOn(
    typeof(AbpIdentityServerDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class AbpIdentityServerEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<IIdentityServerBuilder>(
            builder =>
            {
                builder.AddAbpStores();
            }
        );
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityServerDbContext>(options =>
        {
            options.AddDefaultRepositories<IIdentityServerDbContext>();

            options.AddRepository<Client, ClientRepository>();
            options.AddRepository<ApiResource, ApiResourceRepository>();
            options.AddRepository<ApiScope, ApiScopeRepository>();
            options.AddRepository<IdentityResource, IdentityResourceRepository>();
            options.AddRepository<PersistedGrant, PersistentGrantRepository>();
            options.AddRepository<DeviceFlowCodes, DeviceFlowCodesRepository>();
        });
    }
}
