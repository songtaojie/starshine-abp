using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// SqlSugar上下文接口
    /// </summary>
    public interface ISqlSugarDbContext : IDisposable
    {
        /// <summary>
        /// SqlSugar上下文对象
        /// </summary>
        ISqlSugarClient Context { get; }

        /// <summary>
        /// 原生 Ado 对象
        /// </summary>
        IAdo Ado { get; }
    }
}
