using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Starshine.Abp.SqlSugarCore.Uow
{
    /// <summary>
    /// 事务api
    /// </summary>
    public class SqlSugarTransactionApi : ITransactionApi, ISupportsRollback
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public ISqlSugarDbContext StarterDbContext { get; }

        /// <summary>
        /// 
        /// </summary>
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="starterDbContext"></param>
        /// <param name="cancellationTokenProvider"></param>
        public SqlSugarTransactionApi(
            ISqlSugarDbContext starterDbContext,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            StarterDbContext = starterDbContext;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            StarterDbContext.Ado.CancellationToken = CancellationTokenProvider.FallbackToProvider(cancellationToken);
            await StarterDbContext.Context.Ado.CommitTranAsync();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            StarterDbContext.Dispose();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            StarterDbContext.Ado.CancellationToken = CancellationTokenProvider.FallbackToProvider(cancellationToken);
            await StarterDbContext.Ado.RollbackTranAsync();
        }
    }

}
