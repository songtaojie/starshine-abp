using Microsoft.Extensions.Options;
using SqlSugar;
using Starshine.Abp.SqlSugarCore.SqlSugarCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

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

        internal Dictionary<MultiTenantDbContextType, Type> DbContextReplacements { get; }

        public DbSettingsOptions() 
        {
            DbContextReplacements = new Dictionary<MultiTenantDbContextType, Type>();
        }

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

        internal Type GetReplacedTypeOrSelf(Type dbContextType, MultiTenancySides multiTenancySides = MultiTenancySides.Both)
        {
            var replacementType = dbContextType;
            while (true)
            {
                var foundType = DbContextReplacements.LastOrDefault(x => x.Key.Type == replacementType && x.Key.MultiTenancySide.HasFlag(multiTenancySides));
                if (!foundType.Equals(default(KeyValuePair<MultiTenantDbContextType, Type>)))
                {
                    if (foundType.Value == dbContextType)
                    {
                        throw new AbpException(
                            "Circular DbContext replacement found for " +
                            dbContextType.AssemblyQualifiedName
                        );
                    }
                    replacementType = foundType.Value;
                }
                else
                {
                    return replacementType;
                }
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
