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
public class EfCoreIdentityRoleRepository : EfCoreRepository<IIdentityDbContext, IdentityRole, Guid>, IIdentityRoleRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public EfCoreIdentityRoleRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider,IAbpLazyServiceProvider abpLazyServiceProvider)
        : base(dbContextProvider)
    {
        LazyServiceProvider = abpLazyServiceProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="normalizedRoleName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentityRole?> FindByNormalizedNameAsync(string normalizedRoleName,bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityRoleWithUserCount>> GetListWithUserCountAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var roles = await GetListInternalAsync(sorting, maxResultCount, skipCount, filter, includeDetails, cancellationToken: cancellationToken);

        var roleIds = roles.Select(x => x.Id).ToList();
        var userCount = await (await GetDbContextAsync()).Set<IdentityUserRole>()
            .Where(userRole => roleIds.Contains(userRole.RoleId))
            .GroupBy(userRole => userRole.RoleId)
            .Select(x => new
            {
                RoleId = x.Key,
                Count = x.Count()
            })
            .ToListAsync(GetCancellationToken(cancellationToken));

        return roles.Select(role => new IdentityRoleWithUserCount(role, userCount.FirstOrDefault(x => x.RoleId == role.Id)?.Count ?? 0)).ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityRole>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await GetListInternalAsync(sorting, maxResultCount, skipCount, filter, includeDetails, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityRole>> GetListAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityRole>> GetDefaultOnesAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(r => r.IsDefault)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<long> GetCountAsync(string? filter = null,CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(),x => x.Name.Contains(filter!) || x.NormalizedName.Contains(filter!))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="claimType"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task RemoveClaimFromAllRolesAsync(string claimType, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var roleClaims = await dbContext.Set<IdentityRoleClaim>().Where(uc => uc.ClaimType == claimType).ToListAsync(cancellationToken: cancellationToken);
        if (roleClaims.Any())
        {
            (await GetDbContextAsync()).Set<IdentityRoleClaim>().RemoveRange(roleClaims);
            if (autoSave)
            {
                await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<List<IdentityRole>> GetListInternalAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter!) ||  x.NormalizedName.Contains(filter!))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityRole.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override async Task<IQueryable<IdentityRole>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
