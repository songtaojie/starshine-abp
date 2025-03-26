using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using Starshine.Abp.Swashbuckle.Builders;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 规范化文档中间件拓展
    /// </summary>
    public static class SwaggerDocumentApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="swaggerConfigure"></param>
        /// <param name="swaggerUIConfigure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStarshineSwagger(this IApplicationBuilder app, Action<SwaggerOptions>? swaggerConfigure = default, Action<SwaggerUIOptions>? swaggerUIConfigure = default)
        {
            var builder = app.ApplicationServices.GetRequiredService<ISwaggerDocumentBuilder>();
            // 配置 Swagger 全局参数
            app.UseSwagger(options => builder.BuildSwagger(options, swaggerConfigure));
            // 配置 Swagger UI 参数
            app.UseSwaggerUI(options => builder.BuildSwaggerUI(options, swaggerUIConfigure));
            return app;
        }

        /// <summary>
        /// 添加规范化文档中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="swaggerConfigure"></param>
        /// <param name="swaggerUIConfigure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStarshineSwaggerKnife4j(this IApplicationBuilder app, Action<SwaggerOptions>? swaggerConfigure = default, Action<Knife4UIOptions>? swaggerUIConfigure = default)
        {
            var builder = app.ApplicationServices.GetRequiredService<ISwaggerDocumentBuilder>();
            // 配置 Swagger 全局参数
            app.UseSwagger(options => builder.BuildSwagger(options, swaggerConfigure));

            // 配置 Swagger UI 参数
            app.UseKnife4UI(options => builder.BuildSwaggerKnife4jUI(options, swaggerUIConfigure));
            return app;
        }
    }
}