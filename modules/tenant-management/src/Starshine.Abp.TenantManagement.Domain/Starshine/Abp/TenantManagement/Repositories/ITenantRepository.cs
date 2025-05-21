using Starshine.Abp.Domain.Repositories;
using Starshine.Abp.TenantManagement.Entities;

namespace Starshine.Abp.TenantManagement.Repositories;

/// <summary>
/// 租户存储库
/// </summary>
public interface ITenantRepository : IBasicRepository<Tenant, Guid>
{
    /// <summary>
    /// 按名称查找
    /// </summary>
    /// <param name="normalizedName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Tenant?> FindByNameAsync(string normalizedName,bool includeDetails = true,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="takeCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Tenant>> GetListAsync(
        string? sorting = null,
        int takeCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(string? filter = null,CancellationToken cancellationToken = default);
}
