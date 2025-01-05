using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 
/// </summary>
[DependsOn(typeof(AbpDddApplicationContractsModule))]
[DependsOn(typeof(StarshinePermissionManagementDomainSharedModule))]
[DependsOn(typeof(AbpAuthorizationAbstractionsModule))]
public class StarshinePermissionManagementApplicationContractsModule : AbpModule
{

}
