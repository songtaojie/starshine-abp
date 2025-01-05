using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
public class EfCoreOrganizationUnitRepository
    : EfCoreRepository<IIdentityDbContext, OrganizationUnit, Guid>,
        IOrganizationUnitRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public EfCoreOrganizationUnitRepository( IDbContextProvider<IIdentityDbContext> dbContextProvider,IAbpLazyServiceProvider abpLazyServiceProvider)
        : base(dbContextProvider)
    {
        LazyServiceProvider = abpLazyServiceProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<OrganizationUnit>> GetChildrenAsync(
        Guid? parentId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.ParentId == parentId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="parentId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(
        string code,
        Guid? parentId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<OrganizationUnit>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(OrganizationUnit.DisplayName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<OrganizationUnit>> GetListAsync(
        IEnumerable<Guid> ids,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(t => ids.Contains(t.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<OrganizationUnit>> GetListByRoleIdAsync(
        Guid roleId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = from organizationRole in dbContext.Set<OrganizationUnitRole>()
                    join organizationUnit in dbContext.OrganizationUnits.IncludeDetails(includeDetails) on organizationRole.OrganizationUnitId equals organizationUnit.Id
                    where organizationRole.RoleId == roleId
                    select organizationUnit;

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="displayName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<OrganizationUnit?> GetAsync(
        string displayName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                ou => ou.DisplayName == displayName,
                GetCancellationToken(cancellationToken)
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityRole>> GetRolesAsync(
        OrganizationUnit organizationUnit,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = from organizationRole in dbContext.Set<OrganizationUnitRole>()
                    join role in dbContext.Roles.IncludeDetails(includeDetails) on organizationRole.RoleId equals role.Id
                    where organizationRole.OrganizationUnitId == organizationUnit.Id
                    select role;

        query = query
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityRole.Name) : sorting)
            .PageBy(skipCount, maxResultCount);

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnitIds"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityRole>> GetRolesAsync(
        Guid[] organizationUnitIds,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = from organizationRole in dbContext.Set<OrganizationUnitRole>()
                    join role in dbContext.Roles.IncludeDetails(includeDetails) on organizationRole.RoleId equals role.Id
                    where organizationUnitIds.Contains(organizationRole.OrganizationUnitId)
                    select role;

        query = query
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityRole.Name) : sorting)
            .PageBy(skipCount, maxResultCount);

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<int> GetRolesCountAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = from organizationRole in dbContext.Set<OrganizationUnitRole>()
                    join role in dbContext.Roles on organizationRole.RoleId equals role.Id
                    where organizationRole.OrganizationUnitId == organizationUnit.Id
                    select role;

        return await query.CountAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityRole>> GetUnaddedRolesAsync(
        OrganizationUnit organizationUnit,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToList();
        var dbContext = await GetDbContextAsync();

        return await dbContext.Roles
            .Where(r => !roleIds.Contains(r.Id))
            .IncludeDetails(includeDetails)
            .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.Name.Contains(filter!))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityRole.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<int> GetUnaddedRolesCountAsync(
        OrganizationUnit organizationUnit,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToList();
        var dbContext = await GetDbContextAsync();

        return await dbContext.Roles
            .Where(r => !roleIds.Contains(r.Id))
            .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.Name.Contains(filter!))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUser>> GetMembersAsync(
        OrganizationUnit organizationUnit,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var query = await CreateGetMembersFilteredQueryAsync(organizationUnit, filter!);

        return await query.IncludeDetails(includeDetails).OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityUser.UserName) : sorting)
                    .PageBy(skipCount, maxResultCount)
                    .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<Guid>> GetMemberIdsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        return await (from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                      join user in dbContext.Users on userOu.UserId equals user.Id
                      where userOu.OrganizationUnitId == id
                      select user.Id).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<int> GetMembersCountAsync(
        OrganizationUnit organizationUnit,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = await CreateGetMembersFilteredQueryAsync(organizationUnit, filter!);

        return await query.CountAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUser>> GetUnaddedUsersAsync(
        OrganizationUnit organizationUnit,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var userIdsInOrganizationUnit = dbContext.Set<IdentityUserOrganizationUnit>()
            .Where(uou => uou.OrganizationUnitId == organizationUnit.Id)
            .Select(uou => uou.UserId);

        var query = dbContext.Users
            .Where(u => !userIdsInOrganizationUnit.Contains(u.Id));

        if (!filter.IsNullOrWhiteSpace())
        {
            query = query.Where(u =>
                u.UserName.Contains(filter) ||
                u.Email.Contains(filter) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            );
        }

        return await query
            .IncludeDetails(includeDetails)
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IdentityUser.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<int> GetUnaddedUsersCountAsync(
        OrganizationUnit organizationUnit,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var userIdsInOrganizationUnit = dbContext.Set<IdentityUserOrganizationUnit>()
            .Where(uou => uou.OrganizationUnitId == organizationUnit.Id)
            .Select(uou => uou.UserId);

        return await dbContext.Users
            .Where(u => !userIdsInOrganizationUnit.Contains(u.Id))
            .WhereIf(!filter.IsNullOrWhiteSpace(), u =>
                u.UserName.Contains(filter!) ||
                u.Email.Contains(filter!) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(filter!)))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override async Task<IQueryable<OrganizationUnit>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task RemoveAllRolesAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default)
    {
        organizationUnit.Roles.Clear();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task RemoveAllMembersAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var ouMembersQuery = await dbContext.Set<IdentityUserOrganizationUnit>()
            .Where(q => q.OrganizationUnitId == organizationUnit.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));

        dbContext.Set<IdentityUserOrganizationUnit>().RemoveRange(ouMembersQuery);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    protected virtual async Task<IQueryable<IdentityUser>> CreateGetMembersFilteredQueryAsync(OrganizationUnit organizationUnit, string? filter = null)
    {
        var dbContext = await GetDbContextAsync();

        var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                    join user in dbContext.Users on userOu.UserId equals user.Id
                    where userOu.OrganizationUnitId == organizationUnit.Id
                    select user;

        if (!filter.IsNullOrWhiteSpace())
        {
            query = query.Where(u =>
                u.UserName.Contains(filter) ||
                u.Email.Contains(filter) ||
                (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
            );
        }

        return query;
    }
}
