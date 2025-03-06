using Starshine.Abp.Application.Localization.Resources;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Starshine.Abp.Application;

/// <summary>
/// Starshine Ddd 应用契约模块
/// </summary>
[DependsOn(
    typeof(AbpLocalizationModule),
    typeof(AbpAuditingContractsModule),
    typeof(AbpDataModule)
    )]
public class StarshineDddApplicationContractsModule : AbpModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<StarshineDddApplicationContractsModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<StarshineDddApplicationContractsResource>("zh-Hans")
                .AddVirtualJson("/Starshine/Abp/Application/Localization/Resources");
        });
    }
}
