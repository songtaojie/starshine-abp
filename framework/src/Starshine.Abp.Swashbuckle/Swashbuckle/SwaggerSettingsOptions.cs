using Starshine.Abp.Swashbuckle.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using SwashbuckleDocExpansion = Swashbuckle.AspNetCore.SwaggerUI.DocExpansion;

namespace Starshine.Abp.Swashbuckle
{
    /// <summary>
    /// 规范化文档Swagger配置选项
    /// </summary>
    public sealed class SwaggerSettingsOptions:IConfigureOptions<SwaggerSettingsOptions>
    {
        /// <summary>
        /// 是否允许启用MiniProfiler，默认为true
        /// </summary>
        public bool? EnabledMiniProfiler { get; set; }
        /// <summary>
        /// 文档标题
        /// </summary>
        public string? DocumentTitle { get; set; }

        /// <summary>
        /// 默认分组名，默认为Default
        /// </summary>
        public string? DefaultGroupName { get; set; }

        /// <summary>
        /// 启用授权支持，默认为true
        /// </summary>
        public bool? EnableAuthorized { get; set; }

        /// <summary>
        /// 格式化为V2版本
        /// </summary>
        public bool? FormatAsV2 { get; set; }

        /// <summary>
        /// 配置规范化文档地址
        /// </summary>
        public string? RoutePrefix { get; set; }

        /// <summary>
        /// 配置虚拟目录
        /// </summary>
        public string? VirtualPath { get; set; }

        /// <summary>
        /// 服务目录（修正 IIS 创建 Application 问题）
        /// </summary>
        public string? ServerDir { get; set; }

        /// <summary>
        /// 文档展开设置
        /// </summary>
        public DocExpansion? DocExpansion { get; set; }

        /// <summary>
        /// XML 描述文件
        /// </summary>
        public string[]? XmlComments { get; set; }

        /// <summary>
        /// 分组信息
        /// </summary>
        public SwaggerOpenApiInfo[]? GroupOpenApiInfos { get; set; }

        /// <summary>
        /// 安全定义
        /// </summary>
        public SwaggerOpenApiSecurityScheme[]? SecurityDefinitions { get; set; }

        /// <summary>
        /// 配置 Servers
        /// </summary>
        public OpenApiServer[]? Servers { get; set; }

        /// <summary>
        /// 隐藏 Servers
        /// </summary>
        public bool? HideServers { get; set; }

        /// <summary>
        /// 默认 swagger.json 路由模板
        /// </summary>
        public string? RouteTemplate { get; set; }

        /// <summary>
        /// 启用枚举 Schema 筛选器
        /// </summary>
        public bool? EnableEnumSchemaFilter { get; set; }

        /// <summary>
        /// 启用标签排序筛选器
        /// </summary>
        public bool? EnableTagsOrderDocumentFilter { get; set; }

        /// <summary>
        /// 启用 All Groups 功能
        /// </summary>
        public bool? EnableAllGroups { get; set; }

        internal Action<SwaggerGenOptions>? SwaggerGenOptionsAction { get; private set; }

        /// <summary>
        /// 后置配置
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerSettingsOptions options)
        {
            options.EnabledMiniProfiler ??= false;
            options.DocumentTitle ??= "Specification Api Document";
            options.DefaultGroupName ??= "Default";
            options.FormatAsV2 ??= false;
            options.RoutePrefix ??= "swagger";
            options.DocExpansion ??= SwashbuckleDocExpansion.List;
            options.GroupOpenApiInfos ??= new SwaggerOpenApiInfo[]
            {
                new SwaggerOpenApiInfo()
                {
                    Group=options.DefaultGroupName
                }
            };

            options.EnableAuthorized ??= true;
            if (options.EnableAuthorized == true)
            {
                options.SecurityDefinitions ??= new SwaggerOpenApiSecurityScheme[]
                {
                    new SwaggerOpenApiSecurityScheme
                    {
                        Id="oauth2",
                        Type= SecuritySchemeType.Http,
                        Name="Authorization",
                        Description="JWT Authorization header using the Bearer scheme.Enter Bearer {Token} in box below (note space between two)",
                        BearerFormat="JWT",
                        Scheme="Bearer",
                        In= ParameterLocation.Header,

                    }
                };
            }

            options.Servers ??= Array.Empty<OpenApiServer>();
            options.HideServers ??= false;
            options.RouteTemplate ??= "swagger/{documentName}/swagger.json";
            options.EnableEnumSchemaFilter ??= true;
            options.EnableTagsOrderDocumentFilter ??= true;
            options.EnableAllGroups ??= false;
        }

        /// <summary>
        /// 配置SwaggerGenOptions
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Configure([NotNull] Action<SwaggerGenOptions> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            SwaggerGenOptionsAction = action;
        }

    }
}