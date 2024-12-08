using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp;

namespace Starshine.Abp.SqlSugarCore.Uow
{
    /// <summary>
    /// DbContextProvider实现
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : ISqlSugarDbContext
    {
        private const string TransactionsNotSupportedWarningMessage = "Current database does not support transactions. Your database may remain in an inconsistent state in an error case.";


        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConnectionStringResolver _connectionStringResolver;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly ICurrentTenant _currentTenant;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="connectionStringResolver"></param>
        /// <param name="cancellationTokenProvider"></param>
        /// <param name="currentTenant"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public UnitOfWorkDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver,
            ICancellationTokenProvider cancellationTokenProvider,
            ICurrentTenant currentTenant,
            IServiceProvider serviceProvider,
            ILogger<UnitOfWorkDbContextProvider<TDbContext>> logger)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            _cancellationTokenProvider = cancellationTokenProvider;
            _currentTenant = currentTenant;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TDbContext> GetDbContextAsync()
        {

            var unitOfWork = _unitOfWorkManager.Current;
          
            var targetDbContextType = typeof(TDbContext);
            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType) ?? ConnectionStrings.DefaultConnectionStringName;
            var connectionString = await ResolveConnectionStringAsync(connectionStringName);
            var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";
            if (unitOfWork == null)
            {
                //throw new AbpException("A DbContext can only be created inside a unit of work!");
                var dbContext = (TDbContext)_serviceProvider.GetRequiredService<ISqlSugarDbContext>();
                return dbContext;
            }

            //从当前工作单元获取db
            var databaseApi = unitOfWork.FindDatabaseApi(dbContextKey);

            if (databaseApi == null)
            {
                databaseApi = new SqlSugarDatabaseApi(
                  await CreateDbContextAsync(unitOfWork, connectionStringName, connectionString)
                );

                //创建的db加入到当前工作单元中
                unitOfWork.AddDatabaseApi(dbContextKey, databaseApi);

            }
            return (TDbContext)((SqlSugarDatabaseApi)databaseApi).DbContext;
        }

        /// <summary>
        /// 创建数据库上下文
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="connectionStringName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
        {
            var creationContext = new SqlSugarDbContextCreationContext(connectionStringName, connectionString);
            //将连接key进行传值
            using (SqlSugarDbContextCreationContext.Use(creationContext))
            {
                var dbContext = await CreateDbContextAsync(unitOfWork);
                return dbContext;
            }
        }

        /// <summary>
        /// 创建数据库上下文
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork)
        {
            return unitOfWork.Options.IsTransactional
                ? await CreateDbContextWithTransactionAsync(unitOfWork)
                : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
        }

        /// <summary>
        /// 创建带事务的数据库上下文
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        protected virtual async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
        {
            var transactionApiKey = $"SqlSugarCore_{SqlSugarDbContextCreationContext.Current.ConnectionString}";
            var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as SqlSugarTransactionApi;

            //该db还没有进行开启事务
            if (activeTransaction == null)
            {
                var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

                try
                {
                    var transaction = new SqlSugarTransactionApi(dbContext, _cancellationTokenProvider);
                    unitOfWork.AddTransactionApi(transactionApiKey, transaction);
                    if (unitOfWork.Options.IsolationLevel.HasValue)
                    {
                        await dbContext.Ado.BeginTranAsync(unitOfWork.Options.IsolationLevel.Value);
                    }
                    else
                    {
                        await dbContext.Ado.BeginTranAsync();
                    }
                    
                }
                catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                {
                    _logger.LogWarning(TransactionsNotSupportedWarningMessage);

                    return dbContext;
                }

                return dbContext;
               
            }
            else
            {
                return (TDbContext)activeTransaction.StarterDbContext;
            }

        }

        /// <summary>
        /// 解析连接字符串
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        protected virtual async Task<string> ResolveConnectionStringAsync(string connectionStringName)
        {
            if (typeof(TDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
            {
                using (_currentTenant.Change(null))
                {
                    return await _connectionStringResolver.ResolveAsync(connectionStringName);
                }
            }

            return await _connectionStringResolver.ResolveAsync(connectionStringName);
        }

    }
}
