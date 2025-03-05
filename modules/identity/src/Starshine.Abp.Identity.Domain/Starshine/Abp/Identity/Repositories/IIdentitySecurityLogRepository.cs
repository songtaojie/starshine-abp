using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份安全日志存储库
/// </summary>
public interface IIdentitySecurityLogRepository : IBasicRepository<IdentitySecurityLog, Guid>
{
    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="applicationName"></param>
    /// <param name="identity"></param>
    /// <param name="action"></param>
    /// <param name="userId"></param>
    /// <param name="userName"></param>
    /// <param name="clientId"></param>
    /// <param name="correlationId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentitySecurityLog>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? applicationName = null,
        string? identity = null,
        string? action = null,
        Guid? userId = null,
        string? userName = null,
        string? clientId = null,
        string? correlationId = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取数量
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="applicationName"></param>
    /// <param name="identity"></param>
    /// <param name="action"></param>
    /// <param name="userId"></param>
    /// <param name="userName"></param>
    /// <param name="clientId"></param>
    /// <param name="correlationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? applicationName = null,
        string? identity = null,
        string? action = null,
        Guid? userId = null,
        string? userName = null,
        string? clientId = null,
        string? correlationId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据id和用户id获取
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IdentitySecurityLog?> GetByUserIdAsync(
        Guid id,
        Guid userId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}
