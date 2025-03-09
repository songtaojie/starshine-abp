using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// ApiScope仓储
/// </summary>
public interface IApiScopeRepository : IBasicRepository<ApiScope, Guid>
{
    /// <summary>
    /// 根据名称查找
    /// </summary>
    /// <param name="scopeName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ApiScope?> FindByNameAsync(
        string scopeName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 根据名称查找APi作用域列表
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ApiScope>> GetListByNameAsync(
        string[] scopeNames,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ApiScope>> GetListAsync(
        string sorting,
        int skipCount,
        int maxResultCount,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 获取总数
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(
        string? filter = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 检查名称是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="expectedId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CheckNameExistAsync(
        string name,
        Guid? expectedId = null,
        CancellationToken cancellationToken = default
    );
}
