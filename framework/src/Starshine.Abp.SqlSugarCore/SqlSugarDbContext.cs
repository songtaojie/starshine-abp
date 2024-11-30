using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Starshine.Abp.SqlSugarCore
{
    public abstract class SqlSugarDbContext : ISqlSugarDbContext, ITransientDependency
    {
        protected IAbpLazyServiceProvider LazyServiceProvider { get; }

        private IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetRequiredService<IGuidGenerator>();
        /// <summary>
        /// SqlSugar 客户端
        /// </summary>
        public ISqlSugarClient SqlSugarClient { get;}

        public SqlSugarDbContext(IAbpLazyServiceProvider lazyServiceProvider)
        {
            LazyServiceProvider = lazyServiceProvider;
            var connectionCreator = _lazyServiceProvider.LazyGetRequiredService<ISqlSugarDbConnectionCreator>();
            connectionCreator.OnSqlSugarClientConfig = OnSqlSugarClientConfig;
            connectionCreator.EntityService = EntityService;
            connectionCreator.DataExecuting = DataExecuting;
            connectionCreator.DataExecuted = DataExecuted;
            connectionCreator.OnLogExecuting = OnLogExecuting;
            connectionCreator.OnLogExecuted = OnLogExecuted;
            SqlSugarClient = new SqlSugarClient(connectionCreator.Build(action: options =>
            {
                options.ConnectionString = GetCurrentConnectionString();
                options.DbType = GetCurrentDbType();
            }));
            //统一使用aop处理
            connectionCreator.SetDbAop(SqlSugarClient);
            //替换默认序列化器
            SqlSugarClient.CurrentConnectionConfig.ConfigureExternalServices.SerializeService = SerializeService;
        }

        /// <summary>
        /// db切换多库支持
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCurrentConnectionString()
        {
            var connectionStringResolver = _lazyServiceProvider.LazyGetRequiredService<IConnectionStringResolver>();
            var connectionString = connectionStringResolver.ResolveAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Check.NotNull(Options.Url, "dbUrl未配置");
            }
            return connectionString!;
        }

        public void BackupDataBase()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetSqlSugarClient(ISqlSugarClient sqlSugarClient)
        {
            throw new NotImplementedException();
        }
    }
}
