﻿using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Starshine.Abp.EntityFrameworkCore.DependencyInjection;

namespace Starshine.Abp.EntityFrameworkCore;

public static class AbpDbContextConfigurationContextPostgreSqlExtensions
{
    [Obsolete("Use 'UseNpgsql(...)' method instead. This will be removed in future versions.")]
    public static DbContextOptionsBuilder UsePostgreSql(
        [NotNull] this AbpDbContextConfigurationContext context,
        Action<NpgsqlDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
    {
        return context.UseNpgsql(postgreSqlOptionsAction);
    }

    public static DbContextOptionsBuilder UseNpgsql(
        [NotNull] this AbpDbContextConfigurationContext context,
        Action<NpgsqlDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseNpgsql(context.ExistingConnection, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                postgreSqlOptionsAction?.Invoke(optionsBuilder);
            });
        }
        else
        {
            return context.DbContextOptions.UseNpgsql(context.ConnectionString, optionsBuilder =>
            {
                optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                postgreSqlOptionsAction?.Invoke(optionsBuilder);
            });
        }
    }
}
