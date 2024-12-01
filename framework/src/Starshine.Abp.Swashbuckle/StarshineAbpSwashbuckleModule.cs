using Starshine.Abp.Core;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Starshine.Abp.Swashbuckle
{
    [DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpAspNetCoreMvcModule))]
    public class StarshineAbpSwashbuckleModule : StarshineAbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<StarshineAbpSwashbuckleModule>();
            });
        }
    }
}
