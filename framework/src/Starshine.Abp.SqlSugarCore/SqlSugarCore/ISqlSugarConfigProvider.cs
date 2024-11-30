using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// SqlSugarConfig配置
    /// </summary>
    public interface ISqlSugarConfigProvider
    {
        /// <summary>
        /// 构建SqlSugarConfig配置
        /// </summary>
        /// <param name="dbConnectionConfig"></param>
        /// <returns></returns>
        Task<ConnectionConfig> BuildConnectionConfig(DbConnectionConfig dbConnectionConfig);
    }
}
