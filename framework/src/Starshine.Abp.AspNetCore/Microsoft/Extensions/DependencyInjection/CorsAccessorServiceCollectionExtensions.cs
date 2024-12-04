using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;
using Starshine.Abp.AspNetCore.Cors;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 跨域访问服务拓展类
    /// </summary>
    public static class CorsAccessorServiceCollectionExtensions
    {
        /// <summary>
        /// 配置跨域
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="action"></param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddCorsAccessor(this IServiceCollection services, Action<CorsSettingsOptions>? action = default)
        {
            // 添加跨域配置选项
            services.AddStarshineOptions<CorsSettingsOptions>();
            if (action != null) services.Configure(action);
            services.AddCors();
            services.Configure<CorsOptions, IOptions<CorsSettingsOptions>>((corsOptions, corsSettingsOptions) =>
            {
                var corsSettings = corsSettingsOptions.Value;
                // 添加策略跨域
                corsOptions.AddPolicy(name: corsSettings.PolicyName!, builder =>
                {
                    CorsAccessorPolicy.SetCorsPolicy(builder, corsSettings);
                });
                // 添加自定义配置
                corsSettings.DefaultConfigureCors?.Invoke(corsOptions);
            });
            return services;
        }
    }
}