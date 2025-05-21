using Starshine.Abp.Application;
using Volo.Abp.Modularity;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 
/// </summary>
[DependsOn(
    typeof(StarshinePermissionManagementDomainModule),
    typeof(StarshinePermissionManagementApplicationContractsModule),
    typeof(StarshineDddApplicationModule)
    )]
public class StarshinePermissionManagementApplicationModule : AbpModule
{

}
