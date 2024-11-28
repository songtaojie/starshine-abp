using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.SqlSugarCore
{
    public interface ISqlSugarDbContext : IDisposable
    {
        ISqlSugarClient SqlSugarClient { get; }

        /// <summary>
        /// 数据库备份
        /// </summary>
        void BackupDataBase();
        void SetSqlSugarClient(ISqlSugarClient sqlSugarClient);
    }
}
