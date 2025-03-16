using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.DependencyInjection;
using Starshine.Abp.Core;
using Microsoft.Extensions.Options;

namespace Starshine.Abp.Swashbuckle
{
    /// <summary>
    /// Swashbuckle模块入口
    /// </summary>
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
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

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = GetApplicationBuilder(context);
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

        public static IApplicationBuilder GetApplicationBuilder(ApplicationInitializationContext context)
        {
            IApplicationBuilder? value = context.ServiceProvider.GetRequiredService<IObjectAccessor<IApplicationBuilder>>().Value;
            Check.NotNull(value, "applicationBuilder");
            return value;
        }
    }
}
