using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// 客户端仓库
/// </summary>
public interface IClientRepository : IBasicRepository<Client, Guid>
{
    /// <summary>
    /// 根据客户端Id查找客户端
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Client?> FindByClientIdAsync(
        string clientId,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 获取客户端列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Client>> GetListAsync(
        string sorting,
        int skipCount,
        int maxResultCount,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 获取客户端数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(
        string? filter = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 获取所有允许的跨域请求
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<string>> GetAllDistinctAllowedCorsOriginsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 检查客户端Id是否存在
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="expectedId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CheckClientIdExistAsync(
        string clientId,
        Guid? expectedId = null,
        CancellationToken cancellationToken = default
    );
}
