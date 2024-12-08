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
    /// <summary>
    /// SqlSugar上下文
    /// </summary>
    public abstract class SqlSugarDbContext : ISqlSugarDbContext
    {
        /// <summary>
        /// 懒加载提供器
        /// </summary>
        protected IAbpLazyServiceProvider LazyServiceProvider { get; }

        /// <summary>
        /// 当前租户
        /// </summary>
        public ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

        /// <summary>
        /// Guid生成器
        /// </summary>
        public IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

        /// <summary>
        /// 数据过滤
        /// </summary>
        public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

        /// <summary>
        /// 是否开启多租户过滤
        /// </summary>
        protected virtual bool IsMultiTenantFilterEnabled => DataFilter?.IsEnabled<IMultiTenant>() ?? false;

        /// <summary>
        /// 是否开启软删除过滤
        /// </summary>
        protected virtual bool IsSoftDeleteFilterEnabled => DataFilter?.IsEnabled<ISoftDelete>() ?? false;

        /// <summary>
        /// 当前时区
        /// </summary>
        public IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

        /// <summary>
        /// 日志记录
        /// </summary>
        public ILogger Logger => LazyServiceProvider.LazyGetService<ILogger<SqlSugarDbContext>>(NullLogger<SqlSugarDbContext>.Instance);

        /// <summary>
        /// 数据库设置
        /// </summary>
        public DbSettingsOptions Options => LazyServiceProvider.LazyGetRequiredService<IOptions<DbSettingsOptions>>().Value;

        /// <summary>
        /// SqlSugar 客户端
        /// </summary>
        public ISqlSugarClient Context { get; }

        /// <summary>
        /// 数据库操作对象
        /// </summary>
        public IAdo Ado => Context.Ado;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lazyServiceProvider"></param>
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

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }

    }
}
