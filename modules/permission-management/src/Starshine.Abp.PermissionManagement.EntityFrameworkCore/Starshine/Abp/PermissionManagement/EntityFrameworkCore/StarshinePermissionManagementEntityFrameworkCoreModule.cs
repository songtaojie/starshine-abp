using Microsoft.Extensions.DependencyInjection;
using Starshine.Abp.EntityFrameworkCore;
using Starshine.Abp.PermissionManagement.Entities;
using Volo.Abp.Modularity;

namespace Starshine.Abp.PermissionManagement.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
[DependsOn(typeof(StarshinePermissionManagementDomainModule))]
[DependsOn(typeof(StarshineEntityFrameworkCoreModule))]
public class StarshinePermissionManagementEntityFrameworkCoreModule : AbpModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PermissionManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IPermissionManagementDbContext>();

            options.AddRepository<PermissionGroupDefinitionRecord, EfCorePermissionGroupDefinitionRecordRepository>();
            options.AddRepository<PermissionDefinitionRecord, EfCorePermissionDefinitionRecordRepository>();
            options.AddRepository<PermissionGrant, EfCorePermissionGrantRepository>();
        });
    }
}
