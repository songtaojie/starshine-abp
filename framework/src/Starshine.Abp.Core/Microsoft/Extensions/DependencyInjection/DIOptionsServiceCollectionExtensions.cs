using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding options services to the DI container.
    /// </summary>
    public static class DIOptionsServiceCollectionExtensions
    {

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="OptionsServiceCollectionExtensions.PostConfigure{TOptions}(IServiceCollection, Action{TOptions})"/>.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <typeparam name="TDep">A dependency used by the action.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Configure<TOptions, TDep>(this IServiceCollection services, Action<TOptions, TDep> configureOptions)
            where TOptions : class
            where TDep : class
            => services.Configure<TOptions, TDep>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run before all <seealso cref="OptionsServiceCollectionExtensions.PostConfigure{TOptions}(IServiceCollection, Action{TOptions})"/>.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <typeparam name="TDep">A dependency used by the action.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection Configure<TOptions, TDep>(this IServiceCollection services, string name, Action<TOptions, TDep> configureOptions)
            where TOptions : class
            where TDep : class
        {
            ThrowIfNull(services);
            ThrowIfNull(configureOptions);

            services.AddOptions<TOptions>(name)
                .Configure<TDep>(configureOptions);
            return services;
        }

        /// <summary>
        /// Registers an action used to initialize a particular type of options.
        /// Note: These are run after all <seealso cref="OptionsServiceCollectionExtensions.Configure{TOptions}(IServiceCollection, Action{TOptions})"/>.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <typeparam name="TDep">A dependency used by the action.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection PostConfigure<TOptions, TDep>(this IServiceCollection services, Action<TOptions, TDep> configureOptions)
            where TOptions : class
            where TDep : class
            => services.PostConfigure<TOptions, TDep>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);

        /// <summary>
        /// Registers an action used to configure a particular type of options.
        /// Note: These are run after all <seealso cref="OptionsServiceCollectionExtensions.Configure{TOptions}(IServiceCollection, Action{TOptions})"/>.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configure.</typeparam>
        /// <typeparam name="TDep">A dependency used by the action.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection PostConfigure<TOptions, TDep>(this IServiceCollection services, string name, Action<TOptions, TDep> configureOptions)
            where TOptions : class
            where TDep : class
        {
            ThrowIfNull(services);
            ThrowIfNull(configureOptions);

            services.AddOptions<TOptions>(name)
                .PostConfigure(configureOptions);
            return services;
        }

        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The reference type argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        internal static void ThrowIfNull(object argument, [CallerArgumentExpression(nameof(argument))] string? paramName = default)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
