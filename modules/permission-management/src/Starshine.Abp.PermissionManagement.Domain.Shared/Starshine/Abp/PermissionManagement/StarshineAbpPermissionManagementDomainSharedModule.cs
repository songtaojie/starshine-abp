using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Starshine.Abp.PermissionManagement.Localization;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限管理领域模块
/// </summary>
[DependsOn(
    typeof(AbpValidationModule)
    )]
public class StarshineAbpPermissionManagementDomainSharedModule : AbpModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<StarshineAbpPermissionManagementDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<StarshineAbpPermissionManagementResource>("zh-Hans")
                .AddBaseTypes(
                    typeof(AbpValidationResource)
                ).AddVirtualJson("/Starshine/Abp/PermissionManagement/Localization/Domain");
        });
    }
}
