using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Starshine.Abp.TenantManagement.EntityFrameworkCore;

/// <summary>
/// 租户管理EntityFrameworkCoreModule
/// </summary>
[DependsOn(typeof(StarshineTenantManagementDomainModule))]
[DependsOn(typeof(AbpEntityFrameworkCoreModule))]
public class StarshineTenantManagementEntityFrameworkCoreModule : AbpModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TenantManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<ITenantManagementDbContext>();
        });
    }
}
