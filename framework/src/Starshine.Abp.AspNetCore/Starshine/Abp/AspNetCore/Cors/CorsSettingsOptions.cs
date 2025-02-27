using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;

namespace Starshine.Abp.AspNetCore.Cors
{
    /// <summary>
    /// 跨域配置选项
    /// </summary>
    public sealed class CorsSettingsOptions : IPostConfigureOptions<CorsSettingsOptions>
    {
        /// <summary>
        /// 策略名称
        /// </summary>
        [Required]
        public string? PolicyName { get; set; }

        /// <summary>
        /// 允许来源域名，没有配置则允许所有来源
        /// </summary>
        public string[]? WithOrigins { get; set; }

        /// <summary>
        /// 请求表头，没有配置则允许所有表头
        /// </summary>
        public string[]? WithHeaders { get; set; }

        /// <summary>
        /// 响应标头
        /// </summary>
        public string[]? WithExposedHeaders { get; set; }

        /// <summary>
        /// 设置跨域允许请求谓词，没有配置则允许所有
        /// </summary>
        public string[]? WithMethods { get; set; }

        /// <summary>
        /// 跨域请求中的凭据
        /// </summary>
        public bool? AllowCredentials { get; set; }

        /// <summary>
        /// 设置预检过期时间
        /// </summary>
        public int? SetPreflightMaxAge { get; set; }

        /// <summary>
        /// 修正前端无法获取 Token 问题
        /// </summary>
        public bool? FixedClientToken { get; set; }

        /// <summary>
        /// 启用 SignalR 跨域支持
        /// </summary>
        public bool? EnabledSignalR { get; set; }

        internal Action<CorsOptions>? DefaultConfigureCors { get; set; }

        /// <summary>
        /// 配置cors
        /// </summary>
        /// <param name="action"></param>
        public void ConfigureCors(Action<CorsOptions> action)
        {
            DefaultConfigureCors = action;
        }

        /// <summary>
        /// 后期配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="options"></param>
        public void PostConfigure(string? name, CorsSettingsOptions options)
        {
            PolicyName ??= "StarshineCors";
            WithOrigins ??= [];
            AllowCredentials ??= true;
            FixedClientToken ??= true;
            EnabledSignalR ??= false;
        }
    }
}