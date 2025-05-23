﻿using System;
using JetBrains.Annotations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace Starshine.Abp.EntityFrameworkCore;

public static class AbpDbContextOptionsPostgreSqlExtensions
{
    [Obsolete("Use 'UseNpgsql(...)' method instead. This will be removed in future versions.")]
    public static void UsePostgreSql(
        [NotNull] this AbpDbContextOptions options,
        Action<NpgsqlDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
    {
        options.Configure(context =>
        {
            context.UseNpgsql(postgreSqlOptionsAction);
        });
    }

    [Obsolete("Use 'UseNpgsql(...)' method instead. This will be removed in future versions.")]
    public static void UsePostgreSql<TDbContext>(
        [NotNull] this AbpDbContextOptions options,
        Action<NpgsqlDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
        where TDbContext : AbpDbContext<TDbContext>
    {
        options.Configure<TDbContext>(context =>
        {
            context.UseNpgsql(postgreSqlOptionsAction);
        });
    }

    public static void UseNpgsql(
        [NotNull] this AbpDbContextOptions options,
        Action<NpgsqlDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
    {
        options.Configure(context =>
        {
            context.UseNpgsql(postgreSqlOptionsAction);
        });
    }

    public static void UseNpgsql<TDbContext>(
        [NotNull] this AbpDbContextOptions options,
        Action<NpgsqlDbContextOptionsBuilder>? postgreSqlOptionsAction = null)
        where TDbContext : AbpDbContext<TDbContext>
    {
        options.Configure<TDbContext>(context =>
        {
            context.UseNpgsql(postgreSqlOptionsAction);
        });
    }
}
