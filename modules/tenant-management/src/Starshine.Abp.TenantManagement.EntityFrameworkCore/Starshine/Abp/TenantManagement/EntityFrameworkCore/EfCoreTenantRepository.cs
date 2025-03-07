using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.TenantManagement.Entities;
using Starshine.Abp.TenantManagement.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.TenantManagement.EntityFrameworkCore;
/// <summary>
/// EfCoreTenantRepository
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="dbContextProvider"></param>
public class EfCoreTenantRepository(IDbContextProvider<ITenantManagementDbContext> dbContextProvider) : EfCoreRepository<ITenantManagementDbContext, Tenant, Guid>(dbContextProvider), ITenantRepository
{

    /// <summary>
    /// 根据名称查找租户
    /// </summary>
    /// <param name="normalizedName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<Tenant?> FindByNameAsync(string normalizedName, bool includeDetails = true,CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(t => t.Id)
            .FirstOrDefaultAsync(t => t.NormalizedName == normalizedName, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 获取租户列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="takeCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<Tenant>> GetListAsync(string? sorting = null,
        int takeCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(filter!)
            )
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(Tenant.Name) : sorting)
            .PageBy(skipCount, takeCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 获取租户数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<long> GetCountAsync(string? filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(filter!)
            ).CountAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 包含详情
    /// </summary>
    /// <returns></returns>
    public override async Task<IQueryable<Tenant>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
