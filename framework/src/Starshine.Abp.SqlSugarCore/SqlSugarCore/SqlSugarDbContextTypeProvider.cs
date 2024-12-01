using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.SqlSugarCore
{
    public class SqlSugarDbContextTypeProvider : ISqlSugarDbContextTypeProvider, ITransientDependency
    {
        private readonly DbSettingsOptions _options;
        private readonly ICurrentTenant _currentTenant;

        public SqlSugarDbContextTypeProvider(IOptions<DbSettingsOptions> options, ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
            _options = options.Value;
        }

        public virtual Type GetDbContextType(Type dbContextType)
        {
            //return _options.GetReplacedTypeOrSelf(dbContextType, _currentTenant.GetMultiTenancySide());
            return dbContextType;
        }
    }
}
