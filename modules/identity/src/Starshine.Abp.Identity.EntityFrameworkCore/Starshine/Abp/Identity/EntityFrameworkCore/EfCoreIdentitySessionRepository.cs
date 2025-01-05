using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Timing;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
public class EfCoreIdentitySessionRepository : EfCoreRepository<IIdentityDbContext, IdentitySession, Guid>, IIdentitySessionRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public EfCoreIdentitySessionRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider,IAbpLazyServiceProvider abpLazyServiceProvider)
        : base(dbContextProvider)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentitySession?> FindAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).FirstOrDefaultAsync(x => x.SessionId == sessionId, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public virtual async Task<IdentitySession> GetAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        var session = await FindAsync(sessionId, cancellationToken);
        if (session == null)
        {
            throw new EntityNotFoundException(typeof(IdentitySession));
        }

        return session;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> ExistAsync(string sessionId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.SessionId == sessionId, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="userId"></param>
    /// <param name="device"></param>
    /// <param name="clientId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentitySession>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? userId = null,
        string? device = null,
        string? clientId = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .WhereIf(!device.IsNullOrWhiteSpace(), x => x.Device == device)
            .WhereIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(IdentitySession.LastAccessed)} desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="device"></param>
    /// <param name="clientId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<long> GetCountAsync(
        Guid? userId = null,
        string? device = null,
        string? clientId = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(userId.HasValue, x => x.UserId == userId)
            .WhereIf(!device.IsNullOrWhiteSpace(), x => x.Device == device)
            .WhereIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="exceptSessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteAllAsync(Guid userId, Guid? exceptSessionId = null, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.UserId == userId && x.Id != exceptSessionId, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="device"></param>
    /// <param name="exceptSessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteAllAsync(Guid userId, string device, Guid? exceptSessionId = null, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.UserId == userId && x.Device == device && x.Id != exceptSessionId, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inactiveTimeSpan"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteAllAsync(TimeSpan inactiveTimeSpan, CancellationToken cancellationToken = default)
    {
        var now = LazyServiceProvider.LazyGetRequiredService<IClock>().Now;
        await DeleteDirectAsync(x => x.LastAccessed == null || x.LastAccessed < now.Subtract(inactiveTimeSpan), cancellationToken: cancellationToken);
    }
}
