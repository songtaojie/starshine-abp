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
    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : ISqlSugarDbContext
    {
        private const string TransactionsNotSupportedWarningMessage = "Current database does not support transactions. Your database may remain in an inconsistent state in an error case.";

        public readonly ILogger<UnitOfWorkDbContextProvider<TDbContext>> Logger;
        protected readonly IServiceProvider ServiceProvider;

        protected readonly IUnitOfWorkManager UnitOfWorkManager;
        protected readonly IConnectionStringResolver ConnectionStringResolver;
        protected readonly ICancellationTokenProvider CancellationTokenProvider;
        protected readonly ICurrentTenant CurrentTenant;

        public UnitOfWorkDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver,
            ICancellationTokenProvider cancellationTokenProvider,
            ICurrentTenant currentTenant,
            IServiceProvider serviceProvider,
            ILogger<UnitOfWorkDbContextProvider<TDbContext>> logger)
        {
            UnitOfWorkManager = unitOfWorkManager;
            ConnectionStringResolver = connectionStringResolver;
            CancellationTokenProvider = cancellationTokenProvider;
            CurrentTenant = currentTenant;
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        public virtual async Task<TDbContext> GetDbContextAsync()
        {

            var unitOfWork = UnitOfWorkManager.Current;
          
            var targetDbContextType = typeof(TDbContext);
            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(targetDbContextType) ?? ConnectionStrings.DefaultConnectionStringName;
            var connectionString = await ResolveConnectionStringAsync(connectionStringName);
            var dbContextKey = $"{targetDbContextType.FullName}_{connectionString}";
            if (unitOfWork == null)
            {
                //throw new AbpException("A DbContext can only be created inside a unit of work!");
                var dbContext = (TDbContext)ServiceProvider.GetRequiredService<ISqlSugarDbContext>();
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

        protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork)
        {
            return unitOfWork.Options.IsTransactional
                ? await CreateDbContextWithTransactionAsync(unitOfWork)
                : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
        }
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
                    var transaction = new SqlSugarTransactionApi(dbContext, CancellationTokenProvider);
                    unitOfWork.AddTransactionApi(transactionApiKey, transaction);
                    if (unitOfWork.Options.IsolationLevel.HasValue)
                    {
                        await dbContext.SqlSugarClient.Ado.BeginTranAsync(unitOfWork.Options.IsolationLevel.Value);
                    }
                    else
                    {
                        await dbContext.SqlSugarClient.Ado.BeginTranAsync();
                    }
                    
                }
                catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                {
                    Logger.LogWarning(TransactionsNotSupportedWarningMessage);

                    return dbContext;
                }

                return dbContext;
               
            }
            else
            {
                return (TDbContext)activeTransaction.StarterDbContext;
            }

        }


        protected virtual async Task<string> ResolveConnectionStringAsync(string connectionStringName)
        {
            if (typeof(TDbContext).IsDefined(typeof(IgnoreMultiTenancyAttribute), false))
            {
                using (CurrentTenant.Change(null))
                {
                    return await ConnectionStringResolver.ResolveAsync(connectionStringName);
                }
            }

            return await ConnectionStringResolver.ResolveAsync(connectionStringName);
        }

    }
}
