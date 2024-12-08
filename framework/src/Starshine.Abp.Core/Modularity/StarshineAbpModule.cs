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
        /// <summary>
        /// 配置
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <typeparam name="TDep"></typeparam>
        /// <param name="configureOptions"></param>
        protected void Configure<TOptions, TDep>(Action<TOptions, TDep> configureOptions)
            where TOptions : class
            where TDep : class
        {
            ServiceConfigurationContext.Services.Configure(configureOptions);
        }

        /// <summary>
        /// 后置配置
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <typeparam name="TDep"></typeparam>
        /// <param name="configureOptions"></param>
        protected void PostConfigure<TOptions, TDep>(Action<TOptions, TDep> configureOptions)
            where TOptions : class
            where TDep : class
        {
            ServiceConfigurationContext.Services.PostConfigure(configureOptions);
        }
    }
}
