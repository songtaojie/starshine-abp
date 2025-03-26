using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.DependencyInjection;
using Starshine.Abp.Core;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;

namespace Starshine.Abp.Swashbuckle
{
    /// <summary>
    /// Swashbuckle模块入口
    /// </summary>
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
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
            context.Services.AddStarshineSwaggerGen();
            context.Services.AddEndpointsApiExplorer();
        }

        /// <summary>
        /// 配置应用程序
        /// </summary>
        /// <param name="context"></param>
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var options = app.ApplicationServices.GetRequiredService<IOptions<SwaggerSettingsOptions>>().Value;
            if (options.SwaggerUI == 2)
            {
                app.UseStarshineSwaggerKnife4j();
            }
            else
            {
                app.UseStarshineSwagger();
            }
        }
    }
}
