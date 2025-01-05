using Volo.Abp.Authorization.Permissions;
using Starshine.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Starshine.Abp.PermissionManagement;

namespace Volo.Abp.PermissionManagement.IdentityServer;

[DependsOn(
    typeof(StarshineIdentityServerDomainSharedModule),
    typeof(StarshinePermissionManagementDomainModule)
)]
public class StarshinePermissionManagementDomainIdentityServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<PermissionManagementOptions>(options =>
        {
            options.ManagementProviders.Add<ClientPermissionManagementProvider>();

            options.ProviderPolicies[ClientPermissionValueProvider.ProviderName] = "IdentityServer.Client.ManagePermissions";
        });
    }
}
