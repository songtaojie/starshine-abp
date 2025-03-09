using Starshine.Abp.IdentityServer.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// ApiResource仓库
/// </summary>
public interface IApiResourceRepository : IBasicRepository<ApiResource, Guid>
{
    /// <summary>
    /// 根据名称查找ApiResource
    /// </summary>
    /// <param name="apiResourceName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ApiResource?> FindByNameAsync(string apiResourceName,bool includeDetails = true,CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据名称查找ApiResource
    /// </summary>
    /// <param name="apiResourceNames"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ApiResource>> FindByNameAsync(string[] apiResourceNames,bool includeDetails = true,CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据Scope查找ApiResource
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ApiResource>> GetListByScopesAsync(string[] scopeNames,bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取ApiResource列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ApiResource>> GetListAsync(
        string sorting,
        int skipCount,
        int maxResultCount,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 获取ApiResource数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(
        string? filter = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 检查ApiResource名称是否存在
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
