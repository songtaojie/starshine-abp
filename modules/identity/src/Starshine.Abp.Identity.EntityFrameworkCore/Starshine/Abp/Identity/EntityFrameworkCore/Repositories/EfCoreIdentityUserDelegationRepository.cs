using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Timing;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
public class EfCoreIdentityUserDelegationRepository : EfCoreRepository<IIdentityDbContext, IdentityUserDelegation, Guid>, IIdentityUserDelegationRepository
{
    /// <summary>
    /// 
    /// </summary>
    protected IClock Clock { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    /// <param name="clock"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public EfCoreIdentityUserDelegationRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider, 
        IClock clock,
        IAbpLazyServiceProvider abpLazyServiceProvider) : base(dbContextProvider)
    {
        Clock = clock;
        LazyServiceProvider = abpLazyServiceProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceUserId"></param>
    /// <param name="targetUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUserDelegation>> GetListAsync(Guid? sourceUserId, Guid? targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .WhereIf(sourceUserId.HasValue, x => x.SourceUserId == sourceUserId)
            .WhereIf(targetUserId.HasValue, x => x.TargetUserId == targetUserId)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUserDelegation>> GetActiveDelegationsAsync(Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .Where(x => x.TargetUserId == targetUserId &&
                        x.StartTime <= Clock.Now &&
                        x.EndTime >= Clock.Now)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentityUserDelegation?> FindActiveDelegationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.StartTime <= Clock.Now &&
                    x.EndTime >= Clock.Now
                , cancellationToken: GetCancellationToken(cancellationToken));
    }
}
