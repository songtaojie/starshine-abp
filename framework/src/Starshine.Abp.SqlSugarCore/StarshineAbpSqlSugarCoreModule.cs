using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using Volo.Abp.Modularity;
using Volo.Abp;
using Volo.Abp.Domain;
using SqlSugar;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Reflection;
using Volo.Abp.Data;
using Starshine.Abp.Core;
using Starshine.Abp.SqlSugarCore.Uow;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// SqlSugar模块
    /// </summary>
    [DependsOn(typeof(AbpDddDomainModule))]
    public class StarshineAbpSqlSugarCoreModule : StarshineAbpModule
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddStarshineOptions<DbSettingsOptions>();
            services.TryAddScoped<ISqlSugarDbContext, SqlSugarDbContext>();
            services.AddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
            Configure<AbpDbConnectionOptions,IOptions<DbSettingsOptions>>((option, dbSettings) => 
            {
                option.ConnectionStrings.Default = dbSettings.Value.ConnectionString; 
            });
            services.AddStarshineSugarDbContext<DefaultSqlSugarDbContext>();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using var scope = context.ServiceProvider.CreateScope();
            var service = scope.ServiceProvider;
            var dbConfig = service.GetRequiredService<IOptions<DbSettingsOptions>>().Value;
            var sqlSugarDbContext = service.GetRequiredService<ISqlSugarDbContext>();
            var logger = service.GetRequiredService<ILogger<StarshineAbpSqlSugarCoreModule>>();
            var sqlSugarClient = sqlSugarDbContext.Context as SqlSugarClient;
            var typeFinder = service.GetRequiredService<ITypeFinder>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("==========数据库连接配置:==========");
            sb.AppendLine($"数据库连接字符串：{dbConfig.ConnectionString}");
            sb.AppendLine($"数据库类型：{dbConfig.DbType.ToString()}");
            sb.AppendLine($"是否开启种子数据：{dbConfig.EnableInitSeed}");
            sb.AppendLine($"是否开启初始化表：{dbConfig.EnableInitDb}");
            sb.AppendLine($"是否启用Sql日志记录：{dbConfig.EnableSqlLog}");
            var dbProvider = sqlSugarClient!.GetConnectionScope(dbConfig.ConfigId);
            SqlSugarConfigProvider.InitDatabase(dbProvider, dbConfig, typeFinder.Types);
            if (dbConfig.EnableInitSeed)
            {
                var dataSeeder = service.GetRequiredService<IDataSeeder>();
                await dataSeeder.SeedAsync();
            }
            sb.AppendLine("===============================");
            logger.LogInformation(sb.ToString());
        }
    }
}
