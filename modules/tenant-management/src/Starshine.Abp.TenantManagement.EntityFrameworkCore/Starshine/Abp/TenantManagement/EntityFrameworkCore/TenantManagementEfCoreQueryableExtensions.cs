using System.Linq;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.TenantManagement.Entities;

namespace Starshine.Abp.TenantManagement.EntityFrameworkCore;
/// <summary>
/// 租户查询扩展
/// </summary>
public static class TenantManagementEfCoreQueryableExtensions
{
    /// <summary>
    /// 包含详情
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="include"></param>
    /// <returns></returns>
    public static IQueryable<Tenant> IncludeDetails(this IQueryable<Tenant> queryable, bool include = true)
    {
        if (!include) return queryable;
        return queryable.Include(x => x.ConnectionStrings);
    }
}
