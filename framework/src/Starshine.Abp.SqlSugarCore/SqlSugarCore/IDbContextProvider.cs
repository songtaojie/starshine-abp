using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// 数据库上下文提供器
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
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
