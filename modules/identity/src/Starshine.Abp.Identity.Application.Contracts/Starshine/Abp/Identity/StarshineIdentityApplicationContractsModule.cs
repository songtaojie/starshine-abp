using Starshine.Abp.ObjectExtending;
using Starshine.Abp.PermissionManagement;
using Starshine.Abp.Users;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Starshine.Abp.Identity;
/// <summary>
/// 模块
/// </summary>
[DependsOn(
    typeof(StarshineIdentityDomainSharedModule),
    typeof(StarshineUsersAbstractionModule),
    typeof(AbpAuthorizationModule),
    typeof(StarshinePermissionManagementApplicationContractsModule)
    )]
public class StarshineIdentityApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();
    /// <summary>
    /// 服务配置
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }
    /// <summary>
    /// 后置配置
    /// </summary>
    /// <param name="context"></param>
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.Role,
                getApiTypes: new[] { typeof(IdentityRoleDto) },
                createApiTypes: new[] { typeof(IdentityRoleCreateDto) },
                updateApiTypes: new[] { typeof(IdentityRoleUpdateDto) }
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.User,
                getApiTypes: new[] { typeof(IdentityUserDto) },
                createApiTypes: new[] { typeof(IdentityUserCreateDto) },
                updateApiTypes: new[] { typeof(IdentityUserUpdateDto) }
            );
        });
    }
}
