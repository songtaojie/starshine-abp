using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Core
{
    /// <summary>
    /// StarshineAbp模块
    /// </summary>
    public abstract class StarshineAbpModule:AbpModule
    {
        protected void Configure<TOptions, TDep>(Action<TOptions, TDep> configureOptions)
            where TOptions : class
            where TDep : class
        {
            ServiceConfigurationContext.Services.Configure(configureOptions);
        }

        protected void PostConfigure<TOptions, TDep>(Action<TOptions, TDep> configureOptions)
            where TOptions : class
            where TDep : class
        {
            ServiceConfigurationContext.Services.PostConfigure(configureOptions);
        }
    }
}
