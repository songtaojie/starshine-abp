using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Starshine.Abp.IdentityServer.Entities;
using Starshine.Abp.IdentityServer.Repositories;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore;
/// <summary>
/// IdentityServer EntityFrameworkCore 模块
/// </summary>
[DependsOn(
    typeof(StarshineIdentityServerDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class StarshineIdentityServerEntityFrameworkCoreModule : AbpModule
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="context"></param>
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<IIdentityServerBuilder>(
            builder =>
            {
                builder.AddStores();
            }
        );
    }
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
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
