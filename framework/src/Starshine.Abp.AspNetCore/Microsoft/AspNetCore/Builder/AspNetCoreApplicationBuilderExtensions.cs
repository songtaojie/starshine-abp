// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

public static class AspNetCoreApplicationBuilderExtensions
{
    /// <summary>
    /// Maps endpoints configured with the <see cref="StarshineEndpointRouterOptions"/>.
    /// It internally uses the standard app.UseEndpoints(...) method.
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <param name="additionalConfigurationAction">Additional (and optional) endpoint configuration</param>
    /// <returns></returns>
    public static IApplicationBuilder UseConfiguredEndpoints(
        this IApplicationBuilder app,
        Action<IEndpointRouteBuilder>? additionalConfigurationAction = null)
    {
        var options = app.ApplicationServices
            .GetRequiredService<IOptions<StarshineEndpointRouterOptions>>()
            .Value;

        if (!options.EndpointConfigureActions.Any() && additionalConfigurationAction == null)
        {
            return app;
        }

        return app.UseEndpoints(endpoints =>
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (options.EndpointConfigureActions.Any())
                {
                    var context = new EndpointRouteBuilderContext(endpoints, scope.ServiceProvider);

                    foreach (var configureAction in options.EndpointConfigureActions)
                    {
                        configureAction(context);
                    }
                }
            }

            additionalConfigurationAction?.Invoke(endpoints);
        });
    }
}