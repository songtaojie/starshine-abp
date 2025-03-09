using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;

namespace Starshine.Abp.IdentityServer.Jwt;

//TODO: Should we move this to another package..?
/// <summary>
/// JwtToken中间件
/// </summary>
public static class JwtTokenMiddleware
{
    /// <summary>
    /// 添加JwtToken中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="schema"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseJwtTokenMiddleware(this IApplicationBuilder app, string schema)
    {
        return app.Use(async (ctx, next) =>
        {
            if (ctx.User.Identity?.IsAuthenticated != true)
            {
                var result = await ctx.AuthenticateAsync(schema);
                if (result.Succeeded && result.Principal != null)
                {
                    ctx.User = result.Principal;
                }
            }

            await next();
        });
    }
}
