using Volo.Abp.Authorization.Permissions;
using Starshine.Abp.IdentityServer;
using Volo.Abp.Modularity;

namespace Starshine.Abp.PermissionManagement.IdentityServer;

/// <summary>
/// IdentityServer模块的权限管理模块
/// </summary>
[DependsOn(
    typeof(StarshineIdentityServerDomainSharedModule),
    typeof(StarshinePermissionManagementDomainModule)
)]
public class StarshinePermissionManagementDomainIdentityServerModule : AbpModule
{
    /// <summary>
    /// 配置权限管理
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<ClientPermissionManagementProvider>();

            options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = "IdentityServer.Client.ManagePermissions";
        });
    }
}
