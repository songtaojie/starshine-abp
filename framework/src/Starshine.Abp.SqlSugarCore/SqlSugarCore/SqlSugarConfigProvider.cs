using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// SqlSugar配置初始化
    /// </summary>
    internal static class SqlSugarConfigProvider
    {
        internal const string DefaultConfigId = $"starshine_sqlsugar_configId";

        /// <summary>
        /// 配置连接属性
        /// </summary>
        /// <param name="config"></param>
        /// <param name="serializeService"></param>
        internal static ConnectionConfig SetDbConfig(DbSettingsOptions config, ISerializeService? serializeService = null)
        {
            serializeService ??= DefaultServices.Serialize;
            var configureExternalServices = new ConfigureExternalServices
            {
                EntityNameService = (type, entity) => // 处理表
                {
                    if (config.EnableUnderLine && !entity.DbTableName.Contains('_'))
                        entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线

                },
                EntityService = (type, column) => // 处理列
                {
                    if (type.GetIndexParameters().Length > 0)
                    {
                        column.IsIgnore = true;
                        return;
                    }
                    if (column.IsPrimarykey == false && type.PropertyType.IsGenericType
                        && type.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        column.IsNullable = true;
                    }
                    

                    if (config.DbType == DbType.Oracle)
                    {
                        if (type.PropertyType == typeof(long) || type.PropertyType == typeof(long?))
                            column.DataType = "number(18)";
                        if (type.PropertyType == typeof(bool) || type.PropertyType == typeof(bool?))
                            column.DataType = "number(1)";
                    }
                    if (!type.IsDefined(typeof(SugarColumn), false))
                    {
                        if (config.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                            column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线
                        if (type.ReflectedType?.IsAssignableTo<ISoftDelete>() ?? false && type.Name.Equals(nameof(ISoftDelete.IsDeleted)))
                        {
                            column.DefaultValue = "false";
                            column.ColumnDescription = "是否删除";
                        }
                        else if (type.ReflectedType?.IsAssignableTo<IHasDeletionTime>() ?? false && type.Name.Equals(nameof(IHasDeletionTime.DeletionTime)))
                        {
                            column.IsNullable = true;
                            column.ColumnDescription = "删除时间";
                            column.DefaultValue = "false";
                        }
                        else if (type.ReflectedType?.IsAssignableTo<IHasDeletionTime>() ?? false && type.Name.Equals(nameof(IHasDeletionTime.DeletionTime)))
                        {
                            column.IsNullable = true;
                            column.ColumnDescription = "删除时间";
                            column.DefaultValue = "false";
                        }
                        else if (type.ReflectedType?.IsAssignableTo<IHasCreationTime>() ?? false && type.Name.Equals(nameof(IHasCreationTime.CreationTime)))
                        {
                            column.IsNullable = false;
                            column.ColumnDescription = "创建时间";
                            column.DefaultValue = "false";
                        }
                       
                    }
                },
                SerializeService = serializeService
            };

            config.ConfigureExternalServices = configureExternalServices;
            config.InitKeyType = InitKeyType.Attribute;
            config.IsAutoCloseConnection = true;
            config.MoreSettings = new ConnMoreSettings
            {
                IsAutoRemoveDataCache = true,
                SqlServerCodeFirstNvarchar = true // 采用Nvarchar
            };
            return config;
        }

        /// <summary>
        /// 配置Aop日志
        /// </summary>
        /// <param name="aop"></param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        internal static void SetAopLog(AopProvider aop, DbSettingsOptions config, ILogger logger)
        {
            if (config.EnableSqlLog)
            {
                // 打印SQL语句
                aop.OnLogExecuting = (sql, pars) =>
                {
                    logger.LogInformation($"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetNativeSql(sql, pars)}\r\n");
                };
                aop.OnError = ex =>
                {
                    if (ex.Parametres == null) return;
                    logger.LogError($"【{DateTime.Now}——错误SQL】\r\n {UtilMethods.GetNativeSql(ex.Sql, ex.Parametres as SugarParameter[])} \r\n");
                };
            }


            if (!config.EnableDiffLog) return;
            //开启库表差异化日志
            aop.OnDiffLogEvent = u =>
            {
                var logDiff = JsonConvert.SerializeObject(u);
                logger.LogInformation(DateTime.Now + $"\r\n*****差异日志开始*****\r\n{Environment.NewLine}{logDiff}{Environment.NewLine}*****差异日志结束*****\r\n");
            };
        }

        /// <summary>
        /// 初始化数据库和种子数据
        /// DbConnectionConfig需开启相应的开关
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="config"></param>
        /// <param name="types"></param>
        internal static void InitDatabase(ISqlSugarClient dbProvider, DbSettingsOptions config,IEnumerable<Type> types)
        {
            if (!config.EnableInitDb) return;
            var configId = config.ConfigId.ToString();
            // 创建数据库
            if (config.DbType != DbType.Oracle)
                dbProvider.DbMaintenance.CreateDatabase();

            // 获取所有实体表-初始化表结构
            var entityTypes = types.Where(u => u.IsDefined(typeof(SugarTable), false)).ToList();
            if (!entityTypes.Any()) return;
            foreach (var entityType in entityTypes)
            {
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if (tAtt != null && tAtt.configId?.ToString() == configId || tAtt == null && configId == DefaultConfigId)
                {
                    var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
                    if (splitTable == null)
                        dbProvider.CodeFirst.InitTables(entityType);
                    else
                        dbProvider.CodeFirst.SplitTables().InitTables(entityType);
                }
            }
        }

        /// <summary>
        /// 初始化租户业务数据库
        /// </summary>
        /// <param name="iTenant"></param>
        /// <param name="config"></param>
        /// <param name="types"></param>
        internal static void InitTenantDatabase(ITenant iTenant, DbSettingsOptions config,IEnumerable<Type> types)
        {
            SetDbConfig(config);

            iTenant.AddConnection(config);
            var db = iTenant.GetConnectionScope(config.ConfigId);
            db.DbMaintenance.CreateDatabase();

            // 获取所有实体表-初始化租户业务表
            var entityTypes = types.Where(u => u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(TenantAttribute), false)).ToList();
            if (!entityTypes.Any()) return;
            foreach (var entityType in entityTypes)
            {
                var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
                if (splitTable == null)
                    db.CodeFirst.InitTables(entityType);
                else
                    db.CodeFirst.SplitTables().InitTables(entityType);
            }
        }
    }
}
