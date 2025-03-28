using System;
using System.Collections.Generic;
using System.Linq;
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
public class EfCoreIdentityLinkUserRepository : EfCoreRepository<IIdentityDbContext, IdentityLinkUser, Guid>, IIdentityLinkUserRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public EfCoreIdentityLinkUserRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider,IAbpLazyServiceProvider abpLazyServiceProvider)
        : base(dbContextProvider)
    {
        LazyServiceProvider = abpLazyServiceProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceLinkUserInfo"></param>
    /// <param name="targetLinkUserInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentityLinkUser?> FindAsync(IdentityLinkUserInfo sourceLinkUserInfo, IdentityLinkUserInfo targetLinkUserInfo, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AsNoTracking()
            .OrderBy(x => x.Id).FirstOrDefaultAsync(x =>
                x.SourceUserId == sourceLinkUserInfo.UserId && x.SourceTenantId == sourceLinkUserInfo.TenantId &&
                x.TargetUserId == targetLinkUserInfo.UserId && x.TargetTenantId == targetLinkUserInfo.TenantId ||
                x.TargetUserId == sourceLinkUserInfo.UserId && x.TargetTenantId == sourceLinkUserInfo.TenantId &&
                x.SourceUserId == targetLinkUserInfo.UserId && x.SourceTenantId == targetLinkUserInfo.TenantId
            , cancellationToken: GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="linkUserInfo"></param>
    /// <param name="excludes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityLinkUser>> GetListAsync(IdentityLinkUserInfo linkUserInfo, List<IdentityLinkUserInfo>? excludes = null,CancellationToken cancellationToken = default)
    {
        var query = (await GetDbSetAsync())
            .AsNoTracking()
            .Where(x =>
                x.SourceUserId == linkUserInfo.UserId && x.SourceTenantId == linkUserInfo.TenantId ||
                x.TargetUserId == linkUserInfo.UserId && x.TargetTenantId == linkUserInfo.TenantId);

        if (!excludes.IsNullOrEmpty())
        {
            foreach (var userInfo in excludes!)
            {
                query = query.Where(x =>
                    (x.SourceTenantId != userInfo.TenantId || x.SourceUserId != userInfo.UserId) &&
                    (x.TargetTenantId != userInfo.TenantId || x.TargetUserId != userInfo.UserId));
            }
        }

        return await query.ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="linkUserInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(IdentityLinkUserInfo linkUserInfo, CancellationToken cancellationToken = default)
    {
        var linkUsers = await (await GetDbSetAsync()).AsNoTracking().Where(x =>
                x.SourceUserId == linkUserInfo.UserId && x.SourceTenantId == linkUserInfo.TenantId ||
                x.TargetUserId == linkUserInfo.UserId && x.TargetTenantId == linkUserInfo.TenantId)
            .ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));

        await DeleteManyAsync(linkUsers, cancellationToken: cancellationToken);
    }
}
