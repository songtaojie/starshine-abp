using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Uow;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// 数据库api
    /// </summary>
    public class SqlSugarDatabaseApi : IDatabaseApi, ISupportsSavingChanges
    {
        /// <summary>
        /// 上下文
        /// </summary>
        public ISqlSugarDbContext DbContext { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public SqlSugarDatabaseApi(ISqlSugarDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }
    }
}
