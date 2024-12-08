using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// 默认的SqlSugarDbContext
    /// </summary>
    public class DefaultSqlSugarDbContext : SqlSugarDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lazyServiceProvider"></param>
        public DefaultSqlSugarDbContext(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
        {
        }
    }
}
