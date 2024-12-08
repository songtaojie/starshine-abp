using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// 数据库上下文对象
    /// </summary>
    public interface ISqlSugarDbContextTypeProvider
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        Type GetDbContextType(Type dbContextType);
    }
}
