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
    /// <summary>
    /// 数据库上下文类型
    /// </summary>
    public class SqlSugarDbContextTypeProvider : ISqlSugarDbContextTypeProvider, ITransientDependency
    {
        private readonly DbSettingsOptions _options;
        private readonly ICurrentTenant _currentTenant;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="currentTenant"></param>
        public SqlSugarDbContextTypeProvider(IOptions<DbSettingsOptions> options, ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
            _options = options.Value;
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="dbContextType"></param>
        /// <returns></returns>
        public virtual Type GetDbContextType(Type dbContextType)
        {
            //return _options.GetReplacedTypeOrSelf(dbContextType, _currentTenant.GetMultiTenancySide());
            return dbContextType;
        }
    }
}
