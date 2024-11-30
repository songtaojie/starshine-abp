using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.SqlSugarCore
{
    public interface ISqlSugarDbContextTypeProvider
    {
        Type GetDbContextType(Type dbContextType);
    }
}
