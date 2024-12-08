using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public sealed class DbSettingsOptions : ConnectionConfig, IConfigureOptions<DbSettingsOptions>
    {
        private readonly ILogger _logger;

        /// <summary>
        /// 数据库连接配置
        /// </summary>
        /// <param name="logger"></param>
        public DbSettingsOptions(ILogger<DbSettingsOptions> logger) 
        {
            _logger = logger;
        }
        /// <summary>
        /// 启用库表初始化
        /// </summary>
        public bool EnableInitDb { get; set; }

        /// <summary>
        /// 启用种子初始化
        /// </summary>
        public bool EnableInitSeed { get; set; }

        /// <summary>
        /// 启用驼峰转下划线
        /// </summary>
        public bool EnableUnderLine { get; set; }

        /// <summary>
        /// 启用库表差异日志
        /// </summary>
        public bool EnableDiffLog { get; set; }

        /// <summary>
        /// 启用Sql日志记录
        /// </summary>
        public bool EnableSqlLog { get; set; }

        internal Action<AopProvider>? DefaultConfigureAop { get; set; }

        /// <summary>
        /// 配置aop
        /// </summary>
        /// <param name="action"></param>
        public void ConfigureAop([NotNull] Action<AopProvider> action)
        {
            Volo.Abp.Check.NotNull(action, nameof(action));
            DefaultConfigureAop = action;
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="options"></param>
        public void Configure(DbSettingsOptions options)
        {
            SqlSugarConfigProvider.SetDbConfig(options);
            options.DefaultConfigureAop ??= aop =>
            {
                SqlSugarConfigProvider.SetAopLog(aop, options, _logger);
            };
        }
    }
}
