using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Starshine.Abp.PermissionManagement;

[DependsOn(
    typeof(AbpPermissionManagementDomainModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule)
    )]
public class AbpPermissionManagementApplicationModule : AbpModule
{

}
