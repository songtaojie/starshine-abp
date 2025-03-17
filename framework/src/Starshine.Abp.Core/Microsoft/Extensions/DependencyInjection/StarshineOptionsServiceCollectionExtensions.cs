using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 可变选项服务拓展类
    /// </summary>
    public static class StarshineOptionsServiceCollectionExtensions
    {
        /// <summary>
        /// 添加选项配置
        /// </summary>
        /// <typeparam name="TOptions">选项类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddStarshineOptions<TOptions>(this IServiceCollection services)
            where TOptions : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var optionsType = typeof(TOptions);
            var optionsSettings = optionsType.GetCustomAttribute<OptionsRuleAttribute>(false);

            // 获取键名
            var jsonKey = GetOptionsJsonKey(optionsSettings, optionsType);
            var optionsBuilder = services.AddOptions<TOptions>()
                .BindConfiguration(jsonKey, options =>
                {
                    options.BindNonPublicProperties = true; // 绑定私有变量
                });
            // 配置后期配置
            if (typeof(IPostConfigureOptions<TOptions>).IsAssignableFrom(optionsType))
            {
                var postConfigureMethod = optionsType.GetMethod(nameof(IPostConfigureOptions<TOptions>.PostConfigure));
                if (postConfigureMethod != null)
                {
                    if (optionsSettings?.PostConfigureAll != true)
                        optionsBuilder.PostConfigure(options => postConfigureMethod.Invoke(options, new object[] { optionsType.Name, options }));
                    else
                        services.PostConfigureAll<TOptions>(options => postConfigureMethod.Invoke(options, new object[] { optionsType.Name, options }));
                }
            }
            // 配置后期配置
            if (typeof(IConfigureOptions<TOptions>).IsAssignableFrom(optionsType))
            {
                var configureMethod = optionsType.GetMethod(nameof(IConfigureOptions<TOptions>.Configure), new[] { optionsType } );
                if (configureMethod != null)
                {
                    optionsBuilder.Configure(options => configureMethod.Invoke(options,new object[] { options}));
                }
            }

            return services;
        }


        /// <summary>
        /// 添加选项配置，放在AddAppSettings之后
        /// </summary>
        /// <typeparam name="TOptions">选项类型</typeparam>
        /// <typeparam name="TDep"></typeparam>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddStarshineOptions<TOptions, TDep>(this IServiceCollection services)
            where TOptions : class
            where TDep : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var optionsType = typeof(TOptions);
            var optionsSettings = optionsType.GetCustomAttribute<OptionsRuleAttribute>(false);

            // 获取键名
            var jsonKey = GetOptionsJsonKey(optionsSettings, optionsType);
            var optionsBuilder = services.AddOptions<TOptions>()
                .BindConfiguration(jsonKey, options =>
                {
                    options.BindNonPublicProperties = true; // 绑定私有变量
                });
            // 配置后期配置
            if (typeof(IPostConfigureOptions<TOptions>).IsAssignableFrom(optionsType))
            {
                var postConfigureMethod = optionsType.GetMethod(nameof(IPostConfigureOptions<TOptions>.PostConfigure));
                if (postConfigureMethod != null)
                {
                    if (optionsSettings?.PostConfigureAll != true)
                        optionsBuilder.PostConfigure<TDep>((options, _) => postConfigureMethod.Invoke(options, new object[] { optionsType.Name, options }));
                    else
                        optionsBuilder.PostConfigure<TDep>((options, _) => postConfigureMethod.Invoke(options, new object[] { optionsType.Name, options }));
                }
                //services.AddSingleton(typeof(IPostConfigureOptions<TOptions>), optionsType);
            }
            // 配置后期配置
            if (typeof(IConfigureOptions<TOptions>).IsAssignableFrom(optionsType))
            {
                services.AddSingleton(typeof(IConfigureOptions<TOptions>), optionsType);
            }

            return services;
        }

        /// <summary>
        /// 获取选项键
        /// </summary>
        /// <param name="optionsSettings">选项配置特性</param>
        /// <param name="optionsType">选项类型</param>
        /// <returns></returns>
        private static string GetOptionsJsonKey(OptionsRuleAttribute? optionsSettings, Type optionsType)
        {
            // 默认后缀
            var defaultStuffx = nameof(Options);

            return optionsSettings switch
            {
                // // 没有贴 [OptionsSettings]，如果选项类以 `Options` 结尾，则移除，否则返回类名称
                null => optionsType.Name.EndsWith(defaultStuffx) ? optionsType.Name[0..^defaultStuffx.Length] : optionsType.Name,
                // 如果贴有 [OptionsSettings] 特性，但未指定 JsonKey 参数，则直接返回类名，否则返回 JsonKey
                _ => string.IsNullOrWhiteSpace(optionsSettings.JsonKey) ? optionsType.Name : optionsSettings.JsonKey,
            };
        }
    }
}
