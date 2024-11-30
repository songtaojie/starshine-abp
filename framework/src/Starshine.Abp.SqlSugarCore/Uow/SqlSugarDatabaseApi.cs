using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Uow;

namespace Starshine.Abp.SqlSugarCore
{
    public class SqlSugarDatabaseApi : IDatabaseApi, ISupportsSavingChanges
    {
        public ISqlSugarDbContext DbContext { get; }

        public SqlSugarDatabaseApi(ISqlSugarDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
