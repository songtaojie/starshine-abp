using Starshine.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限管理应用契约
/// </summary>
[DependsOn(typeof(StarshineDddApplicationContractsModule))]
[DependsOn(typeof(StarshinePermissionManagementDomainSharedModule))]
[DependsOn(typeof(AbpAuthorizationAbstractionsModule))]
public class StarshinePermissionManagementApplicationContractsModule : AbpModule
{

}
