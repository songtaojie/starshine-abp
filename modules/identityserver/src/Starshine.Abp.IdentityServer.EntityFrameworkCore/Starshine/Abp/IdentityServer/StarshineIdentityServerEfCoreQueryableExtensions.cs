using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// EF Core扩展
/// </summary>
public static class StarshineIdentityServerEfCoreQueryableExtensions
{
    /// <summary>
    /// 获取ApiResource的详细信息
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    public static IQueryable<ApiResource> IncludeDetails(this IQueryable<ApiResource> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Secrets)
            .Include(x => x.UserClaims)
            .Include(x => x.Scopes)
            .Include(x => x.Properties);
    }
    /// <summary>
    /// 获取ApiScope的详细信息
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    public static IQueryable<ApiScope> IncludeDetails(this IQueryable<ApiScope> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.UserClaims)
            .Include(x => x.Properties);
    }

    /// <summary>
    /// 获取IdentityResource的详细信息
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    public static IQueryable<IdentityResource> IncludeDetails(this IQueryable<IdentityResource> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.UserClaims)
            .Include(x => x.Properties);
    }
    /// <summary>
    /// 获取Client的详细信息
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    public static IQueryable<Client> IncludeDetails(this IQueryable<Client> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.AllowedGrantTypes)
            .Include(x => x.RedirectUris)
            .Include(x => x.PostLogoutRedirectUris)
            .Include(x => x.AllowedScopes)
            .Include(x => x.ClientSecrets)
            .Include(x => x.Claims)
            .Include(x => x.IdentityProviderRestrictions)
            .Include(x => x.AllowedCorsOrigins)
            .Include(x => x.Properties);
    }
}
