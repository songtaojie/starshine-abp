using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 
/// </summary>
[DependsOn(
    typeof(StarshineAbpPermissionManagementDomainModule),
    typeof(StarshineAbpPermissionManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule)
    )]
public class StarshineAbpPermissionManagementApplicationModule : AbpModule
{

}
