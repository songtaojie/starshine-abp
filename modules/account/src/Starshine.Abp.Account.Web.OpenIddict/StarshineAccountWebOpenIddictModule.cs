using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.VirtualFileSystem;

namespace Starshine.Abp.Account.Web;

[DependsOn(
    typeof(StarshineAccountWebModule),
    typeof(AbpOpenIddictAspNetCoreModule)
)]
public class StarshineAccountWebOpenIddictModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(StarshineAccountWebOpenIddictModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<StarshineAccountWebOpenIddictModule>();
        });
    }
}
