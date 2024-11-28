using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// SqlSugar配置初始化
    /// </summary>
    internal static class SqlSugarConfigProvider
    {
        internal const string DefaultConfigId = $"starshine_sqlsugar_configId";

        /// <summary>
        /// 
        /// </summary>
        private static IEnumerable<Type>? _effectiveTypes;

        /// <summary>
        /// SqlSugar配置初始化
        /// </summary>
        static SqlSugarConfigProvider()
        {
        }

        /// <summary>
        /// 有效程序集类型
        /// </summary>
        internal static IEnumerable<Type> EffectiveTypes
        {
            get
            {
                if (_effectiveTypes != null && _effectiveTypes.Any()) return _effectiveTypes;
                var assemblies = GetAssemblies();
                if (assemblies != null)
                {
                    _effectiveTypes = assemblies.SelectMany(GetTypes);
                }
                else
                {
                    _effectiveTypes = Array.Empty<Type>();
                }
                return _effectiveTypes;
            }
        }

        /// <summary>
        /// 加载程序集中的所有类型
        /// </summary>
        /// <param name="ass"></param>
        /// <returns></returns>
        private static IEnumerable<Type> GetTypes(Assembly ass)
        {
            return ass.GetTypes().Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsPublic && !u.IsDefined(typeof(SkipScanAttribute), false));
        }


        /// <summary>
        /// 配置连接属性
        /// </summary>
        /// <param name="config"></param>
        internal static DbConnectionConfig SetDbConfig(DbConnectionConfig config)
        {
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
                    if (config.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                        column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线

                    if (config.DbType == DbType.Oracle)
                    {
                        if (type.PropertyType == typeof(long) || type.PropertyType == typeof(long?))
                            column.DataType = "number(18)";
                        if (type.PropertyType == typeof(bool) || type.PropertyType == typeof(bool?))
                            column.DataType = "number(1)";
                    }

                    if (typeof(IEntity<>).IsAssignableFromGenericType(type.ReflectedType)
                        && !type.IsDefined(typeof(SugarColumn), false))
                    {
                        switch (type.Name)
                        {
                            case nameof(EntityBase.Id):
                                column.IsPrimarykey = true;
                                if (type.PropertyType == typeof(string))
                                {
                                    column.Length = 36;
                                }
                                break;
                            case nameof(CreationEntityBase.CreatorId):
                                if (type.PropertyType == typeof(string))
                                {
                                    column.Length = 36;
                                }
                                break;
                            case nameof(Starshine.Common.AuditedEntityBase.UpdaterId):
                                if (type.PropertyType == typeof(string))
                                {
                                    column.Length = 36;
                                }
                                break;
                            case nameof(FullAuditedEntityBase.DeleterId):
                                if (type.PropertyType == typeof(string))
                                {
                                    column.Length = 36;
                                }
                                break;
                        }
                    }
                }
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
        /// <param name="db"></param>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        internal static void SetAopLog(ISqlSugarClient db, DbConnectionConfig config, ILogger logger)
        {
            if (config.EnableSqlLog)
            {
                // 设置超时时间
                db.Ado.CommandTimeOut = 30;

                // 打印SQL语句
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    using (logger.BeginScope(new Dictionary<string, object>
                    {
                        [SugarLogScope.Source] = SugarLogScope.SourceValue,
                        [SugarLogScope.Sql] = sql,
                        [SugarLogScope.SqlPars] = JsonSerializer.Serialize(pars),
                        [SugarLogScope.LogType] = 1,
                        [SugarLogScope.SugarActionType] = db.SugarActionType,
                    }))
                    {
                        logger.LogInformation($"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetNativeSql(sql, pars)}\r\n");
                    }
                };
                db.Aop.OnError = ex =>
                {
                    if (ex.Parametres == null) return;
                    using (logger.BeginScope(new Dictionary<string, object>
                    {
                        [SugarLogScope.Source] = SugarLogScope.SourceValue,
                        [SugarLogScope.Sql] = ex.Sql,
                        [SugarLogScope.SqlPars] = JsonSerializer.Serialize(ex.Parametres),
                        [SugarLogScope.LogType] = 2,
                        [SugarLogScope.SugarActionType] = db.SugarActionType,
                    }))
                    {
                        logger.LogError($"【{DateTime.Now}——错误SQL】\r\n {UtilMethods.GetNativeSql(ex.Sql, ex.Parametres as SugarParameter[])} \r\n");
                    }
                };
            }


            if (!config.EnableDiffLog) return;
            //开启库表差异化日志
            db.Aop.OnDiffLogEvent = u =>
            {
                var logDiff = JsonSerializer.Serialize(u);
                using (logger.BeginScope(new Dictionary<string, object>
                {
                    [SugarLogScope.Source] = SugarLogScope.SourceValue,
                    [SugarLogScope.Sql] = logDiff,
                    [SugarLogScope.LogType] = 3,
                    [SugarLogScope.SugarActionType] = db.SugarActionType,
                }))
                {
                    logger.LogInformation(DateTime.Now + $"\r\n*****差异日志开始*****\r\n{Environment.NewLine}{logDiff}{Environment.NewLine}*****差异日志结束*****\r\n");
                }
            };
        }

        private static List<string?> _isInitDbList = new List<string?>();
        /// <summary>
        /// 初始化数据库和种子数据
        /// DbConnectionConfig需开启相应的开关
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="config"></param>
        public static void InitDatabase(ISqlSugarClient dbProvider, DbConnectionConfig config)
        {
            if (!config.EnableInitDb) return;
            var configId = config.ConfigId.ToString();
            if (_isInitDbList.Contains(configId)) return;
            _isInitDbList.Add(configId);
            // 创建数据库
            if (config.DbType != DbType.Oracle)
                dbProvider.DbMaintenance.CreateDatabase();

            // 获取所有实体表-初始化表结构
            var entityTypes = EffectiveTypes.Where(u => u.IsDefined(typeof(SugarTable), false)).ToList();
            if (!entityTypes.Any()) return;
            foreach (var entityType in entityTypes)
            {
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if ((tAtt != null && tAtt.configId?.ToString() == configId) || (tAtt == null && configId == DefaultConfigId))
                {
                    var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
                    if (splitTable == null)
                        dbProvider.CodeFirst.InitTables(entityType);
                    else
                        dbProvider.CodeFirst.SplitTables().InitTables(entityType);
                }
            }

            if (!config!.EnableInitSeed) return;
            // 获取所有种子配置-初始化数据
            var seedDataTypes = EffectiveTypes.Where(u => u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>)))).ToList();
            if (!seedDataTypes.Any()) return;
            foreach (var seedType in seedDataTypes)
            {
                var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                var allowSeedData = (tAtt != null && tAtt.configId.ToString() == configId) || (tAtt == null && configId == DefaultConfigId);
                if (!allowSeedData) continue;
                var hasDataMethod = seedType.GetMethod(nameof(ISqlSugarEntitySeedData<object>.HasData));
                if (hasDataMethod == null) continue;
                var instance = Activator.CreateInstance(seedType);
                if (instance == null) continue;
                var seedData = ((IEnumerable?)hasDataMethod.Invoke(instance, null))?.Cast<object>();
                if (seedData == null) continue;

                var entityInfo = dbProvider.EntityMaintenance.GetEntityInfoNoCache(entityType);
                if (entityInfo.Columns.Any(u => u.IsPrimarykey))
                {
                    // 按主键进行批量增加和更新
                    dbProvider.InsertableByObject(seedData.ToList()).ExecuteCommand();
                    var storage = dbProvider.StorageableByObject(seedData.ToList()).ToStorage();
                    storage.AsInsertable.ExecuteCommand();
                    var ignoreUpdate = hasDataMethod.GetCustomAttribute<IgnoreSeedUpdateAttribute>();
                    if (ignoreUpdate == null) storage.AsUpdateable.ExecuteCommand();
                }
                else
                {
                    // 无主键则只进行插入
                    if (!dbProvider.Queryable(entityInfo.DbTableName, entityInfo.DbTableName).Any())
                        dbProvider.InsertableByObject(seedData.ToList()).ExecuteCommand();
                }
            }
        }


        /// <summary>
        /// 初始化租户业务数据库
        /// </summary>
        /// <param name="iTenant"></param>
        /// <param name="config"></param>
        public static void InitTenantDatabase(ITenant iTenant, DbConnectionConfig config)
        {
            SetDbConfig(config);

            iTenant.AddConnection(config);
            var db = iTenant.GetConnectionScope(config.ConfigId);
            db.DbMaintenance.CreateDatabase();

            // 获取所有实体表-初始化租户业务表
            var entityTypes = EffectiveTypes.Where(u => u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(TenantAttribute), false)).ToList();
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


        /// <summary>
        /// 框架 App 静态类
        /// </summary>
        internal static Type? StarshineApp { get; set; }

        /// <summary>
        /// 获取框架上下文
        /// </summary>
        /// <returns></returns>
        internal static Assembly? GetFrameworkContext(Assembly? callAssembly)
        {
            if (callAssembly == null) return null;
            if (StarshineApp != null) return StarshineApp.Assembly;

            var hxCoreAssemblyName = callAssembly.GetReferencedAssemblies()
                                                       .FirstOrDefault(u => u.Name == nameof(Starshine));
            if (hxCoreAssemblyName == null) return null;
            var hxCoreAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(hxCoreAssemblyName);

            // 获取 App 静态类
            StarshineApp = hxCoreAssembly.GetType("Starshine.StarshineApp");

            return hxCoreAssembly;
        }


        private static IEnumerable<Assembly> GetAssemblies()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var frameworkAssembly = GetFrameworkContext(entryAssembly);
            IEnumerable<Assembly>? result = null;
            if (frameworkAssembly != null)
            {
                var assembliesField = StarshineApp!.GetField("Assemblies", BindingFlags.Public | BindingFlags.Static);
                if (assembliesField != null)
                {
                    result = assembliesField.GetValue(null) as IEnumerable<Assembly>;
                }
            }
            if (result == null)
            {
                var refAssembyNames = entryAssembly!.GetReferencedAssemblies();
                result = refAssembyNames.Select(AssemblyLoadContext.Default.LoadFromAssemblyName);
            }
            return result;
        }
    }
}
