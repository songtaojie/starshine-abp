using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.Caching.FreeRedis
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public sealed class CacheSettingsOptions
    {
        /// <summary>
        /// redis配置
        /// </summary>
        public RedisCacheSettingsOptions? Redis { get; set; }
    }

    /// <summary>
    /// redis缓存配置
    /// </summary>
    public sealed class RedisCacheSettingsOptions
    {

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 用于连接到Redis的配置。
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Slave连接字符串
        /// </summary>
        public IEnumerable<string>? SlaveConnectionStrings { get; set; }
    }
}
