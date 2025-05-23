﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Starshine.Abp.EntityFrameworkCore.DependencyInjection;

namespace Starshine.Abp.EntityFrameworkCore;

public static class AbpDbContextConfigurationContextMySQLExtensions
{
    public static DbContextOptionsBuilder UseMySQL(
       [NotNull] this AbpDbContextConfigurationContext context,
       Action<MySqlDbContextOptionsBuilder>? mySQLOptionsAction = null)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseMySql(context.ExistingConnection,
                ServerVersion.AutoDetect(context.ConnectionString), optionsBuilder =>
                {
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    mySQLOptionsAction?.Invoke(optionsBuilder);
                });
        }
        else
        {
            return context.DbContextOptions.UseMySql(context.ConnectionString,
                ServerVersion.AutoDetect(context.ConnectionString), optionsBuilder =>
                {
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    mySQLOptionsAction?.Invoke(optionsBuilder);
                });
        }
    }
}
