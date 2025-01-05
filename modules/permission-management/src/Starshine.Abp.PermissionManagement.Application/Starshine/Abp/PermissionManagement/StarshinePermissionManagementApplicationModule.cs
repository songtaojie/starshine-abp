using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 
/// </summary>
[DependsOn(
    typeof(StarshinePermissionManagementDomainModule),
    typeof(StarshinePermissionManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule)
    )]
public class StarshinePermissionManagementApplicationModule : AbpModule
{

}
