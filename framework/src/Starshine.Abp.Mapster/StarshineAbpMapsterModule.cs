using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Starshine.Abp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Reflection;

namespace Starshine.Abp.Mapster
{
    /// <summary>
    /// Mapster模块
    /// </summary>
    [DependsOn(
    typeof(AbpObjectMappingModule))]
    public class StarshineAbpMapsterModule: StarshineAbpModule
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="context"></param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMapsterObjectMapper();

            // 配置支持依赖注入
            context.Services.AddSingleton(sp =>
            {
                // 获取全局映射配置
                var config = TypeAdapterConfig.GlobalSettings;
                var assemblyFinder = sp.GetRequiredService<IAssemblyFinder>();
                // 扫描所有继承  IRegister 接口的对象映射配置
                if (assemblyFinder.Assemblies.Any()) 
                    config.Scan(assemblyFinder.Assemblies.ToArray());

                // 配置默认全局映射（支持覆盖）
                config.Default
                      .NameMatchingStrategy(NameMatchingStrategy.Flexible)
                      .PreserveReference(true);

                // 配置默认全局映射（忽略大小写敏感）
                config.Default
                      .NameMatchingStrategy(NameMatchingStrategy.IgnoreCase)
                      .PreserveReference(true);
                return config;
            });

            context.Services.AddTransient<IMapper, ServiceMapper>();
            context.Services.AddTransient<IMapperAccessor>(sp => new MapperAccessor()
            {
                Mapper = sp.GetRequiredService<IMapper>()
            });
        }
    }
}
