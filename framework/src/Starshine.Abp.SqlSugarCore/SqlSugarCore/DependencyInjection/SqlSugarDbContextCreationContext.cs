using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// SqlSugar上下文创建
    /// </summary>
    public class SqlSugarDbContextCreationContext
    {
        /// <summary>
        /// 当前上下文
        /// </summary>
        public static SqlSugarDbContextCreationContext Current => _current.Value!;
        private static readonly AsyncLocal<SqlSugarDbContextCreationContext> _current = new AsyncLocal<SqlSugarDbContextCreationContext>();

        /// <summary>
        /// 连接字符串名字
        /// </summary>
        public string ConnectionStringName { get; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <param name="connectionString"></param>
        public SqlSugarDbContextCreationContext(string connectionStringName, string connectionString)
        {
            ConnectionStringName = connectionStringName;
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 使用当前上下文
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IDisposable Use(SqlSugarDbContextCreationContext context)
        {
            var previousValue = Current;
            _current.Value = context;
            return new DisposeAction(() => _current.Value = previousValue);
        }
    }
}
