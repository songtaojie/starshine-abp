using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Starshine.Abp.Identity.EntityFrameworkCore;
/// <summary>
/// 
/// </summary>
public static class IdentityEfCoreQueryableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    public static IQueryable<IdentityUser> IncludeDetails(this IQueryable<IdentityUser> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Roles)
            .Include(x => x.Logins)
            .Include(x => x.Claims)
            .Include(x => x.Tokens)
            .Include(x => x.OrganizationUnits);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    public static IQueryable<IdentityRole> IncludeDetails(this IQueryable<IdentityRole> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Claims);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    public static IQueryable<OrganizationUnit> IncludeDetails(this IQueryable<OrganizationUnit> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.Roles);
    }
}
