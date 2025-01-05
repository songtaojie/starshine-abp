using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Extensions.DependencyInjection;
/// <summary>
/// 
/// </summary>
public static class StarshineIdentityServiceCollectionExtensions
{
    /// <summary>
    /// 持有者转发身份认证
    /// </summary>
    /// <param name="services"></param>
    /// <param name="jwtBearerScheme"></param>
    /// <returns></returns>
    public static IServiceCollection ForwardIdentityAuthenticationForBearer(this IServiceCollection services, string jwtBearerScheme = "Bearer")
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.ForwardDefaultSelector = ctx =>
            {
                string? authorization = ctx.Request.Headers.Authorization;
                if (!authorization.IsNullOrWhiteSpace() && authorization.StartsWith($"{jwtBearerScheme} ", StringComparison.OrdinalIgnoreCase))
                {
                    return jwtBearerScheme;
                }

                return null;
            };
        });

        return services;
    }
}
