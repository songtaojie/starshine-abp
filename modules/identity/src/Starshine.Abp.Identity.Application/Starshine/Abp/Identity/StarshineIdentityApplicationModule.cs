using Starshine.Abp.PermissionManagement;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
[DependsOn(
    typeof(StarshineIdentityDomainModule),
    typeof(StarshineIdentityApplicationContractsModule),
    typeof(StarshinePermissionManagementApplicationModule)
    )]
public class StarshineIdentityApplicationModule : AbpModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }
}
