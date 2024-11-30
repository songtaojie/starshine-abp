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
    public class SqlSugarTransactionApi : ITransactionApi, ISupportsRollback
    {
        public ISqlSugarDbContext StarterDbContext { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public SqlSugarTransactionApi(
            ISqlSugarDbContext starterDbContext,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            StarterDbContext = starterDbContext;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            var token = CancellationTokenProvider.FallbackToProvider(cancellationToken);
            token.ThrowIfCancellationRequested();
            await StarterDbContext.SqlSugarClient.Ado.CommitTranAsync();
        }

        public void Dispose()
        {
            StarterDbContext.Dispose();
            StarterDbContext.SqlSugarClient.Ado.BeginTranAsync();
            StarterDbContext.SqlSugarClient.Ado.Connection
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            var token = CancellationTokenProvider.FallbackToProvider(cancellationToken);
            token.ThrowIfCancellationRequested();
            await StarterDbContext.SqlSugarClient.Ado.RollbackTranAsync();
        }
    }

}
