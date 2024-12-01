using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Threading;

namespace Starshine.Abp.SqlSugarCore
{
    public abstract class SqlSugarDbContext : ISqlSugarDbContext
    {
        protected IAbpLazyServiceProvider LazyServiceProvider { get; }

        public ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

        public IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

        public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

        protected virtual Guid? CurrentTenantId => CurrentTenant?.Id;

        protected virtual bool IsMultiTenantFilterEnabled => DataFilter?.IsEnabled<IMultiTenant>() ?? false;

        protected virtual bool IsSoftDeleteFilterEnabled => DataFilter?.IsEnabled<ISoftDelete>() ?? false;

        public IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

        public ILogger Logger => LazyServiceProvider.LazyGetService<ILogger<SqlSugarDbContext>>(NullLogger<SqlSugarDbContext>.Instance);

        public DbSettingsOptions Options => LazyServiceProvider.LazyGetRequiredService<IOptions<DbSettingsOptions>>().Value;

        /// <summary>
        /// SqlSugar 客户端
        /// </summary>
        public ISqlSugarClient Context { get; }

        public IAdo Ado => Context.Ado;

        public SqlSugarDbContext(IAbpLazyServiceProvider lazyServiceProvider)
        {
            LazyServiceProvider = lazyServiceProvider;
            Context = BuildSqlSugarClient();
        }

        private ISqlSugarClient BuildSqlSugarClient()
        {
            Options.ConnectionString = GetConnectionString();
            SqlSugarClient sugarClient = new SqlSugarClient(Options);
            Options.DefaultConfigureAop?.Invoke(sugarClient.Aop);
            return sugarClient;
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string GetConnectionString()
        {
            var connectionStringResolver = LazyServiceProvider.LazyGetRequiredService<IConnectionStringResolver>();
            var connectionString =
                AsyncHelper.RunSync(() => connectionStringResolver.ResolveAsync());
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = Options.ConnectionString;
            }
            return connectionString!;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

    }
}
