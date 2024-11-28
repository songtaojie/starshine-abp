using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public class DbSettingsOptions : IConfigureOptions<DbSettingsOptions>
    {
        /// <summary>
        /// 数据库连接配置
        /// </summary>
        public IEnumerable<DbConnectionConfig>? ConnectionConfigs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public void Configure(DbSettingsOptions options)
        {
            options.ConnectionConfigs ??= new List<DbConnectionConfig>();
            foreach (var dbConfig in options.ConnectionConfigs)
            {
                if (dbConfig.ConfigId == null || string.IsNullOrWhiteSpace(dbConfig.ConfigId.ToString()))
                    dbConfig.ConfigId = SqlSugarConfigProvider.DefaultConfigId;
            }
        }
    }

    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public sealed class DbConnectionConfig : ConnectionConfig
    {
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

        internal ConnectionConfig ToConnectionConfig()
        {
            return new ConnectionConfig
            {
                AopEvents = AopEvents,
                ConfigId = ConfigId,
                ConfigureExternalServices = ConfigureExternalServices,
                ConnectionString = ConnectionString,
                DbLinkName = DbLinkName,
                DbType = DbType,
                IndexSuffix = IndexSuffix,
                IsAutoCloseConnection = IsAutoCloseConnection,
                LanguageType = LanguageType,
                MoreSettings = MoreSettings,
                SlaveConnectionConfigs = SlaveConnectionConfigs,
                SqlMiddle = SqlMiddle
            };
        }
    }
}
