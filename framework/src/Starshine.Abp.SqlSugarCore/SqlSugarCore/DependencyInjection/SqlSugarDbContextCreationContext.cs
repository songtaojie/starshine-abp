﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Starshine.Abp.SqlSugarCore
{
    public class SqlSugarDbContextCreationContext
    {
        public static SqlSugarDbContextCreationContext Current => _current.Value!;
        private static readonly AsyncLocal<SqlSugarDbContextCreationContext> _current = new AsyncLocal<SqlSugarDbContextCreationContext>();

        public string ConnectionStringName { get; }

        public string ConnectionString { get; }

        public DbConnection? ExistingConnection { get; internal set; }

        public SqlSugarDbContextCreationContext(string connectionStringName, string connectionString)
        {
            ConnectionStringName = connectionStringName;
            ConnectionString = connectionString;
        }

        public static IDisposable Use(SqlSugarDbContextCreationContext context)
        {
            var previousValue = Current;
            _current.Value = context;
            return new DisposeAction(() => _current.Value = previousValue);
        }
    }
}
