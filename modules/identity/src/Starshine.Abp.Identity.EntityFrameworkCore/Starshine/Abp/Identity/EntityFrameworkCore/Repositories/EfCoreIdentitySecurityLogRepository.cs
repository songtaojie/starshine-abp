using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.Identity.EntityFrameworkCore;
/// <summary>
/// 
/// </summary>
public class EfCoreIdentitySecurityLogRepository : EfCoreRepository<IIdentityDbContext, IdentitySecurityLog, Guid>, IIdentitySecurityLogRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public EfCoreIdentitySecurityLogRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider,IAbpLazyServiceProvider abpLazyServiceProvider)
        : base(dbContextProvider)
    {
        LazyServiceProvider = abpLazyServiceProvider;
    }

    /// <summary>
    /// 
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
    public virtual async Task<List<IdentitySecurityLog>> GetListAsync(
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
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var query = await GetListQueryAsync(
            startTime,
            endTime,
            applicationName,
            identity,
            action,
            userId,
            userName,
            clientId,
            correlationId,
            cancellationToken
        );

        return await query.OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(IdentitySecurityLog.CreationTime)} desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 
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
    public virtual async Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? applicationName = null,
        string? identity = null,
        string? action = null,
        Guid? userId = null,
        string? userName = null,
        string? clientId = null,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var query = await GetListQueryAsync(
            startTime,
            endTime,
            applicationName,
            identity,
            action,
            userId,
            userName,
            clientId,
            correlationId,
            cancellationToken
        );

        return await query.LongCountAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentitySecurityLog?> GetByUserIdAsync(Guid id, Guid userId, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
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
    protected virtual async Task<IQueryable<IdentitySecurityLog>> GetListQueryAsync(
          DateTime? startTime = null,
          DateTime? endTime = null,
          string? applicationName = null,
          string? identity = null,
          string? action = null,
          Guid? userId = null,
          string? userName = null,
          string? clientId = null,
          string? correlationId = null,
          CancellationToken cancellationToken = default)
    {
        return (await GetDbSetAsync()).AsNoTracking()
            .WhereIf(startTime.HasValue, securityLog => securityLog.CreationTime >= startTime!.Value)
            .WhereIf(endTime.HasValue, securityLog => securityLog.CreationTime < endTime!.Value.AddDays(1).Date)
            .WhereIf(!applicationName.IsNullOrWhiteSpace(), securityLog => securityLog.ApplicationName == applicationName)
            .WhereIf(!identity.IsNullOrWhiteSpace(), securityLog => securityLog.Identity == identity)
            .WhereIf(!action.IsNullOrWhiteSpace(), securityLog => securityLog.Action == action)
            .WhereIf(userId.HasValue, securityLog => securityLog.UserId == userId)
            .WhereIf(!userName.IsNullOrWhiteSpace(), securityLog => securityLog.UserName == userName)
            .WhereIf(!clientId.IsNullOrWhiteSpace(), securityLog => securityLog.ClientId == clientId)
            .WhereIf(!correlationId.IsNullOrWhiteSpace(), securityLog => securityLog.CorrelationId == correlationId);
    }
}
