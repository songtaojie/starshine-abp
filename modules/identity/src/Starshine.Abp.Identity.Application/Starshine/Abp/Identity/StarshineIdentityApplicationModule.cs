using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份认证应用模块
/// </summary>
[DependsOn(
    typeof(StarshineIdentityDomainModule),
    typeof(StarshineIdentityApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule)
    )]
public class StarshineIdentityApplicationModule : AbpModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }
}
