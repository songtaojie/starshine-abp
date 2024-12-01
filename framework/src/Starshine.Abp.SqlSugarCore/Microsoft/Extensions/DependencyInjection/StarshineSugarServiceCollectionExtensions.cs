using Microsoft.Extensions.DependencyInjection.Extensions;
using Starshine.Abp.SqlSugarCore;
using Starshine.Abp.SqlSugarCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StarshineSugarServiceCollectionExtensions
    {
        public static IServiceCollection AddStarshineSugarDbContext<TDbContext>(this IServiceCollection services, Action<DbSettingsOptions>? action = default)
            where TDbContext : class, ISqlSugarDbContext
        {
            if(action != null)services.Configure<DbSettingsOptions>(action);
            services.AddScoped<ISqlSugarDbContext, TDbContext>();
            services.AddTransient(typeof(IRepository<>), typeof(SqlSugarRepository<>));
            services.AddTransient(typeof(IRepository<,>), typeof(SqlSugarRepository<,>));
            services.AddTransient(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
            services.AddTransient(typeof(ISqlSugarRepository<,>), typeof(SqlSugarRepository<,>));
            return services;
        }
    }
}
