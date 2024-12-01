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
            StarterDbContext.Ado.CancellationToken = CancellationTokenProvider.FallbackToProvider(cancellationToken);
            await StarterDbContext.Context.Ado.CommitTranAsync();
        }

        public void Dispose()
        {
            StarterDbContext.Dispose();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            StarterDbContext.Ado.CancellationToken = CancellationTokenProvider.FallbackToProvider(cancellationToken);
            await StarterDbContext.Ado.RollbackTranAsync();
        }
    }

}
