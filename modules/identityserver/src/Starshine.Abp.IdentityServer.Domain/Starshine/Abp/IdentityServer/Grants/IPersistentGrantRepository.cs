using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.IdentityServer.Grants;
/// <summary>
/// 持久化授权存储
/// </summary>
public interface IPersistentGrantRepository : IBasicRepository<PersistedGrant, Guid>
{
    /// <summary>
    /// 获取授权列表
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="sessionId"></param>
    /// <param name="clientId"></param>
    /// <param name="type"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<PersistedGrant>> GetListAsync(string? subjectId,string? sessionId,string? clientId,string? type, bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据key获取授权
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PersistedGrant?> FindByKeyAsync(string key,CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据subjectId获取授权
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<PersistedGrant>> GetListBySubjectIdAsync(string key,CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据过期时间获取授权
    /// </summary>
    /// <param name="maxExpirationDate"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<PersistedGrant>> GetListByExpirationAsync(DateTimeOffset maxExpirationDate,int maxResultCount,CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除过期授权
    /// </summary>
    /// <param name="maxExpirationDate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteExpirationAsync(DateTimeOffset maxExpirationDate,CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除授权
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="sessionId"></param>
    /// <param name="clientId"></param>
    /// <param name="type"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(
        string? subjectId = null,
        string? sessionId = null,
        string? clientId = null,
        string? type = null,
        CancellationToken cancellationToken = default
    );
}
