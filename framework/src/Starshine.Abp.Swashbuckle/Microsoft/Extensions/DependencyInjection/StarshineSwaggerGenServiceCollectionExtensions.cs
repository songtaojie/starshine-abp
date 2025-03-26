using Starshine.Abp.Swashbuckle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Starshine.Abp.Swashbuckle.Builders;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 规范化接口服务拓展类
    /// </summary>
    public static class StarshineSwaggerGenServiceCollectionExtensions
    {
        /// <summary>
        /// 添加规范化文档服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="action">swagger配置</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddStarshineSwaggerGen(this IServiceCollection services, Action<SwaggerSettingsOptions>? action = default)
        {
            services.AddStarshineOptions<SwaggerSettingsOptions>();
            if (action != null)services.Configure(action);
            services.Configure<SwaggerGenOptions,ISwaggerDocumentBuilder>((options, builder) =>
            {
                builder.BuildSwaggerGen(options);
            });
            services.AddSwaggerGen();
            return services;
        }
    }
}