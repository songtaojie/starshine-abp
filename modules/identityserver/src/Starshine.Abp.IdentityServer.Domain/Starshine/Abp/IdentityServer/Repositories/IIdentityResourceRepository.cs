using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// 身份资源仓储
/// </summary>
public interface IIdentityResourceRepository : IBasicRepository<IdentityResource, Guid>
{
    /// <summary>
    /// 根据scopeName获取身份资源
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityResource>> GetListByScopeNameAsync(
        string[] scopeNames,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 获取身份资源列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityResource>> GetListAsync(
        string sorting,
        int skipCount,
        int maxResultCount,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 获取身份资源数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(
        string? filter = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 根据名称获取身份资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IdentityResource?> FindByNameAsync(
        string name,
        bool includeDetails = true,
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
