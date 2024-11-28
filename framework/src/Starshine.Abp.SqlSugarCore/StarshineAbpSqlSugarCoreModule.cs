using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp;
using Volo.Abp.Domain;
using SqlSugar;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.SqlSugarCore
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class StarshineAbpSqlSugarCoreModule : AbpModule
    {
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = services.GetConfiguration();
            services.AddStarshineOptions<DbSettingsOptions>();

            var section = configuration.GetSection("DbConnOptions");
            Configure<DbConnOptions>(section);
            services.AddScoped<ISqlSugarClient>(provider => InitSqlSugarClient(provider, buildAction));
            services.TryAddScoped<ISqlSugarDbContext, SqlSugarDbContext>();

            //不开放sqlsugarClient
            //service.AddTransient<ISqlSugarClient>(x => x.GetRequiredService<ISqlsugarDbContext>().SqlSugarClient);


            service.AddTransient(typeof(IRepository<>), typeof(SqlSugarRepository<>));
            service.AddTransient(typeof(IRepository<,>), typeof(SqlSugarRepository<,>));
            service.AddTransient(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
            service.AddTransient(typeof(ISqlSugarRepository<,>), typeof(SqlSugarRepository<,>));

            service.AddTransient(typeof(ISugarDbContextProvider<>), typeof(UnitOfWorkSqlsugarDbContextProvider<>));
            //替换Sqlsugar默认序列化器，用来解决.Select()不支持嵌套对象/匿名对象的非公有访问器 值无法绑定,如Id属性
            context.Services.AddSingleton<ISerializeService, SqlSugarNonPublicSerializer>();

            var dbConfig = section.Get<DbConnOptions>();
            //将默认db传递给abp连接字符串模块
            Configure<AbpDbConnectionOptions>(x => { x.ConnectionStrings.Default = dbConfig.Url; });

            return Task.CompletedTask;
        }

        private static ISqlSugarClient InitSqlSugarClient(IServiceProvider provider, Action<ISqlSugarClient, IServiceProvider>? buildAction = default)
        {
            var logger = provider.GetRequiredService<ILogger<ISqlSugarClient>>();
            var options = provider.GetRequiredService<IOptions<DbSettingsOptions>>().Value;
            //options.ConnectionConfigs SetDbConfig
            if (!_isInitialized)
            {
                _isInitialized = true;
                foreach (var item in options.ConnectionConfigs!)
                {
                    SqlSugarConfigProvider.SetDbConfig(item);
                }
                RepositoryExtension.ConnectionConfigs = options.ConnectionConfigs;
            }

            var connectionConfigs = options.ConnectionConfigs!.Select(r => r.ToConnectionConfig()).ToList();
            SqlSugarClient sugarClient = new SqlSugarClient(connectionConfigs, db =>
            {
                foreach (var dbConnectionConfig in options.ConnectionConfigs!)
                {
                    var dbProvider = db.GetConnectionScope(dbConnectionConfig.ConfigId);
                    SqlSugarConfigProvider.SetAopLog(dbProvider, dbConnectionConfig, logger);
                    buildAction?.Invoke(dbProvider, provider);
                    //每次上下文都会执行
                    SqlSugarConfigProvider.InitDatabase(dbProvider, dbConnectionConfig);
                }
            });
            return sugarClient;
        }

        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var service = context.ServiceProvider;
            var options = service.GetRequiredService<IOptions<DbSettingsOptions>>().Value;

            var logger = service.GetRequiredService<ILogger<StarshineAbpSqlSugarCoreModule>>();


            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("==========数据库连接配置:==========");
            foreach (var item in options.ConnectionConfigs!)
            {
                sb.AppendLine($"数据库连接字符串：{item.ConnectionString}");
                sb.AppendLine($"数据库类型：{item.DbType.ToString()}");
                sb.AppendLine($"是否开启种子数据：{item.EnableInitSeed}");
                sb.AppendLine($"是否开启初始化表：{item.EnableInitDb}");
                sb.AppendLine($"是否开启Saas多租户：{options.EnabledSaasMultiTenancy}");
            }
            sb.AppendLine("===============================");


            logger.LogInformation(sb.ToString());
            //Todo：准备支持多租户种子数据及CodeFirst

            if (options.EnabledCodeFirst)
            {
                CodeFirst(service);
            }

            if (options.EnabledDbSeed)
            {
                await DataSeedAsync(service);
            }
        }

        private void CodeFirst(IServiceProvider service)
        {
            var moduleContainer = service.GetRequiredService<IModuleContainer>();
            var db = service.GetRequiredService<ISqlSugarDbContext>().SqlSugarClient;

            //尝试创建数据库
            db.DbMaintenance.CreateDatabase();

            List<Type> types = new List<Type>();
            foreach (var module in moduleContainer.Modules)
            {
                types.AddRange(module.Assembly.GetTypes()
                    .Where(x => x.GetCustomAttribute<IgnoreCodeFirstAttribute>() == null)
                    .Where(x => x.GetCustomAttribute<SugarTable>() != null)
                    .Where(x => x.GetCustomAttribute<SplitTableAttribute>() is null));
            }

            if (types.Count > 0)
            {
                db.CopyNew().CodeFirst.InitTables(types.ToArray());
            }
        }

        private async Task DataSeedAsync(IServiceProvider service)
        {
            var dataSeeder = service.GetRequiredService<IDataSeeder>();
            await dataSeeder.SeedAsync();
        }
    }
}
