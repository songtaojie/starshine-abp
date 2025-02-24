using Starshine.Abp.Core;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Starshine.Abp.Swashbuckle
{
    /// <summary>
    /// Swashbuckle模块入口
    /// </summary>
    [DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpAspNetCoreMvcModule))]
    public class StarshineSwashbuckleModule : StarshineAbpModule
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<StarshineSwashbuckleModule>();
            });
        }
    }
}
