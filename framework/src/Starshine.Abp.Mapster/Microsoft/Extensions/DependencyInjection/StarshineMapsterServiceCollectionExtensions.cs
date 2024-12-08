using Microsoft.Extensions.DependencyInjection.Extensions;
using Starshine.Abp.Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Mapster服务扩展类型
    /// </summary>
    public static class StarshineMapsterServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Mapster对象关系映射服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapsterObjectMapper(this IServiceCollection services)
        {
            return services.Replace(
                ServiceDescriptor.Transient<IAutoObjectMappingProvider, MapsterAutoObjectMappingProvider>()
            );
        }

        /// <summary>
        /// 添加Mapster对象关系映射服务
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMapsterObjectMapper<TContext>(this IServiceCollection services)
        {
            return services.Replace(
                ServiceDescriptor.Transient<IAutoObjectMappingProvider<TContext>, MapsterAutoObjectMappingProvider<TContext>>()
            );
        }
    }
}
