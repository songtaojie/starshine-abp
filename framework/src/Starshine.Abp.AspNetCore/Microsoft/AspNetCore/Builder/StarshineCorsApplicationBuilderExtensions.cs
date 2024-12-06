using Starshine.CorsAccessor.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Starshine.Abp.AspNetCore.Starshine.Abp.AspNetCore.Cors;
using Starshine.Abp.AspNetCore.Cors;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 跨域中间件拓展
    /// </summary>
    public static class StarshineCorsApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加跨域中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorsAccessor(this IApplicationBuilder app)
        {
            // 获取选项
            var options = app.ApplicationServices.GetService<IOptions<CorsSettingsOptions>>();
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Add the AddCorsAccessor method to services");
            var corsAccessorSettings = options.Value;
            if (corsAccessorSettings.EnabledSignalR ?? false)
            {
                // 配置跨域中间件
                app.UseCors(builder =>
                {
                    CorsAccessorPolicy.SetCorsPolicy(builder, corsAccessorSettings, true);
                });
            }
            else
            {
                // 配置跨域中间件
                app.UseCors(corsAccessorSettings.PolicyName!);
            }

            return app;
        }
    }
}