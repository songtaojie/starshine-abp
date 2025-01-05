using Volo.Abp.Authorization.Permissions;
using Starshine.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Starshine.Abp.PermissionManagement;
using Starshine.Abp.Users;

namespace Volo.Abp.PermissionManagement.Identity;
/// <summary>
/// 
/// </summary>
[DependsOn(
    typeof(StarshineIdentityDomainSharedModule),
    typeof(StarshinePermissionManagementDomainModule),
    typeof(StarshineUsersAbstractionModule)
)]
public class StarshinePermissionManagementDomainIdentityModule : AbpModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<UserPermissionManagementProvider>();
            options.ManagementProviders.Add<RolePermissionManagementProvider>();

            //TODO: Can we prevent duplication of permission names without breaking the design and making the system complicated
            options.ProviderPolicies[UserPermissionValueProvider.ProviderName] = "AbpIdentity.Users.ManagePermissions";
            options.ProviderPolicies[RolePermissionValueProvider.ProviderName] = "AbpIdentity.Roles.ManagePermissions";
        });
    }
}
