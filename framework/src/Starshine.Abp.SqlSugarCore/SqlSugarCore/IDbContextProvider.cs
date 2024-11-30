using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.SqlSugarCore
{
    public interface IDbContextProvider<TDbContext>
        where TDbContext : ISqlSugarDbContext
    {
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns></returns>
        Task<TDbContext> GetDbContextAsync();
    }
}
