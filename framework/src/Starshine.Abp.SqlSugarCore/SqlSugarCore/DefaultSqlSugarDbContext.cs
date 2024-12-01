using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.SqlSugarCore
{
    public class DefaultSqlSugarDbContext : SqlSugarDbContext
    {
        public DefaultSqlSugarDbContext(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
        {
        }
    }
}
