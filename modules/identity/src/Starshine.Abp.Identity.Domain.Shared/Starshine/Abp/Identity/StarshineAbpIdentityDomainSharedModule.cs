using Microsoft.Extensions.DependencyInjection;
using Starshine.Abp.Identity.Localization;
using Starshine.Abp.Users;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Starshine.Abp.Identity;

/// <summary>
/// DomainShared模块
/// </summary>
[DependsOn(
    typeof(StarshineAbpUsersDomainSharedModule),
    typeof(AbpValidationModule),
    typeof(AbpFeaturesModule)
    )]
public class StarshineAbpIdentityDomainSharedModule : AbpModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<StarshineAbpIdentityDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<IdentityResource>("zh-Hans")
                .AddBaseTypes(
                    typeof(AbpValidationResource)
                ).AddVirtualJson("/Starshine/Abp/Identity/Localization");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Starshine.Abp.Identity", typeof(IdentityResource));
        });
    }
}
