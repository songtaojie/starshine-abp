﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.Identity.EntityFrameworkCore;
/// <summary>
/// 
/// </summary>
public class EfCoreIdentityUserRepository : EfCoreRepository<IIdentityDbContext, IdentityUser, Guid>, IIdentityUserRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public EfCoreIdentityUserRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider,IAbpLazyServiceProvider abpLazyServiceProvider)
        : base(dbContextProvider)
    {
        LazyServiceProvider = abpLazyServiceProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="normalizedUserName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentityUser?> FindByNormalizedUserNameAsync(
        string normalizedUserName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(
                u => u.NormalizedUserName == normalizedUserName,
                GetCancellationToken(cancellationToken)
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<string>> GetRoleNamesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = from userRole in dbContext.Set<IdentityUserRole>()
                    join role in dbContext.Roles on userRole.RoleId equals role.Id
                    where userRole.UserId == id
                    select role.Name;
        var organizationUnitIds = dbContext.Set<IdentityUserOrganizationUnit>().Where(q => q.UserId == id).Select(q => q.OrganizationUnitId).ToArray();

        var organizationRoleIds = await (
            from ouRole in dbContext.Set<OrganizationUnitRole>()
            join ou in dbContext.Set<OrganizationUnit>() on ouRole.OrganizationUnitId equals ou.Id
            where organizationUnitIds.Contains(ouRole.OrganizationUnitId)
            select ouRole.RoleId
        ).ToListAsync(GetCancellationToken(cancellationToken));

        var orgUnitRoleNameQuery = dbContext.Roles.Where(r => organizationRoleIds.Contains(r.Id)).Select(n => n.Name);
        var resultQuery = query.Union(orgUnitRoleNameQuery);
        return await resultQuery.ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUserIdWithRoleNames>> GetRoleNamesAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var userRoles = await (from userRole in dbContext.Set<IdentityUserRole>()
                               join role in dbContext.Roles on userRole.RoleId equals role.Id
                               where userIds.Contains(userRole.UserId)
                               group new
                               {
                                   userRole.UserId,
                                   role.Name
                               } by userRole.UserId
            into gp
                               select new IdentityUserIdWithRoleNames
                               {
                                   Id = gp.Key,
                                   RoleNames = gp.Select(x => x.Name).ToArray()
                               }).ToListAsync(cancellationToken: cancellationToken);

        var orgUnitRoles = await (from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                                  join roleOu in dbContext.Set<OrganizationUnitRole>() on userOu.OrganizationUnitId equals roleOu.OrganizationUnitId
                                  join role in dbContext.Roles on roleOu.RoleId equals role.Id
                                  where userIds.Contains(userOu.UserId)
                                  group new
                                  {
                                      userOu.UserId,
                                      role.Name
                                  } by userOu.UserId
            into gp
                                  select new IdentityUserIdWithRoleNames
                                  {
                                      Id = gp.Key,
                                      RoleNames = gp.Select(x => x.Name).ToArray()
                                  }).ToListAsync(cancellationToken: cancellationToken);

        return userRoles.Concat(orgUnitRoles).GroupBy(x => x.Id).Select(x => new IdentityUserIdWithRoleNames { Id = x.Key, RoleNames = x.SelectMany(y => y.RoleNames).Distinct().ToArray() }).ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<string>> GetRoleNamesInOrganizationUnitAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                    join roleOu in dbContext.Set<OrganizationUnitRole>() on userOu.OrganizationUnitId equals roleOu.OrganizationUnitId
                    join ou in dbContext.Set<OrganizationUnit>() on roleOu.OrganizationUnitId equals ou.Id
                    join userOuRoles in dbContext.Roles on roleOu.RoleId equals userOuRoles.Id
                    where userOu.UserId == id
                    select userOuRoles.Name;

        var result = await query.ToListAsync(GetCancellationToken(cancellationToken));

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loginProvider"></param>
    /// <param name="providerKey"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentityUser?> FindByLoginAsync( string loginProvider,string providerKey, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="normalizedEmail"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentityUser?> FindByNormalizedEmailAsync(string normalizedEmail, bool includeDetails = true,CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="claim"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUser>> GetListByClaimAsync( Claim claim,bool includeDetails = false,CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="claimType"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task RemoveClaimFromAllUsersAsync(string claimType, bool autoSave, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var userClaims = await dbContext.Set<IdentityUserClaim>().Where(uc => uc.ClaimType == claimType).ToListAsync(cancellationToken: cancellationToken);
        if (userClaims.Any())
        {
            (await GetDbContextAsync()).Set<IdentityUserClaim>().RemoveRange(userClaims);
            if (autoSave)
            {
                await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="normalizedRoleName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
        string normalizedRoleName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var role = await dbContext.Roles
            .Where(x => x.NormalizedName == normalizedRoleName)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

        if (role == null)
        {
            return new List<IdentityUser>();
        }

        return await dbContext.Users
            .IncludeDetails(includeDetails)
            .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<Guid>> GetUserIdListByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == roleId)
            .Select(x => x.UserId).ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="roleId"></param>
    /// <param name="organizationUnitId"></param>
    /// <param name="userName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="isLockedOut"></param>
    /// <param name="notActive"></param>
    /// <param name="emailConfirmed"></param>
    /// <param name="isExternal"></param>
    /// <param name="maxCreationTime"></param>
    /// <param name="minCreationTime"></param>
    /// <param name="maxModifitionTime"></param>
    /// <param name="minModifitionTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUser>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        string? userName = null,
        string? phoneNumber = null,
        string? emailAddress = null,
        string? name = null,
        string? surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetFilteredQueryableAsync(
            filter,
            roleId,
            organizationUnitId,
            userName,
            phoneNumber,
            emailAddress,
            name,
            surname,
            isLockedOut,
            notActive,
            emailConfirmed,
            isExternal,
            maxCreationTime,
            minCreationTime,
            maxModifitionTime,
            minModifitionTime,
            cancellationToken
        );

        return await query.IncludeDetails(includeDetails)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityUser.UserName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityRole>> GetRolesAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = from userRole in dbContext.Set<IdentityUserRole>()
                    join role in dbContext.Roles.IncludeDetails(includeDetails) on userRole.RoleId equals role.Id
                    where userRole.UserId == id
                    select role;

        //TODO: Needs improvement
        var userOrganizationsQuery = from userOrg in dbContext.Set<IdentityUserOrganizationUnit>()
                                     join ou in dbContext.OrganizationUnits.IncludeDetails(includeDetails) on userOrg.OrganizationUnitId equals ou.Id
                                     where userOrg.UserId == id
                                     select ou;

        var orgUserRoleQuery = dbContext.Set<OrganizationUnitRole>()
            .Where(q => userOrganizationsQuery
            .Select(t => t.Id)
            .Contains(q.OrganizationUnitId))
            .Select(t => t.RoleId)
            .ToArray();

        var orgRoles = dbContext.Roles.Where(q => orgUserRoleQuery.Contains(q.Id));
        var resultQuery = query.Union(orgRoles);

        return await resultQuery.ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="roleId"></param>
    /// <param name="organizationUnitId"></param>
    /// <param name="userName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="isLockedOut"></param>
    /// <param name="notActive"></param>
    /// <param name="emailConfirmed"></param>
    /// <param name="isExternal"></param>
    /// <param name="maxCreationTime"></param>
    /// <param name="minCreationTime"></param>
    /// <param name="maxModifitionTime"></param>
    /// <param name="minModifitionTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<long> GetCountAsync(
        string? filter = null,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        string? userName = null,
        string? phoneNumber = null,
        string? emailAddress = null,
        string? name = null,
        string? surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetFilteredQueryableAsync(
            filter,
            roleId,
            organizationUnitId,
            userName,
            phoneNumber,
            emailAddress,
            name,
            surname,
            isLockedOut,
            notActive,
            emailConfirmed,
            isExternal,
            maxCreationTime,
            minCreationTime,
            maxModifitionTime,
            minModifitionTime,
            cancellationToken
        )).LongCountAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                    join ou in dbContext.OrganizationUnits.IncludeDetails(includeDetails) on userOu.OrganizationUnitId equals ou.Id
                    where userOu.UserId == id
                    select ou;

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnitId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(
        Guid organizationUnitId,
        CancellationToken cancellationToken = default
        )
    {
        var dbContext = await GetDbContextAsync();

        var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                    join user in dbContext.Users on userOu.UserId equals user.Id
                    where userOu.OrganizationUnitId == organizationUnitId
                    select user;

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnitIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
        List<Guid> organizationUnitIds,
        CancellationToken cancellationToken = default
        )
    {
        var dbContext = await GetDbContextAsync();

        var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                    join user in dbContext.Users on userOu.UserId equals user.Id
                    where organizationUnitIds.Contains(userOu.OrganizationUnitId)
                    select user;

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitWithChildrenAsync(
        string code,
        CancellationToken cancellationToken = default
        )
    {
        var dbContext = await GetDbContextAsync();

        var query = from userOu in dbContext.Set<IdentityUserOrganizationUnit>()
                    join user in dbContext.Users on userOu.UserId equals user.Id
                    join ou in dbContext.Set<OrganizationUnit>() on userOu.OrganizationUnitId equals ou.Id
                    where ou.Code.StartsWith(code)
                    select user;

        return await query.ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override async Task<IQueryable<IdentityUser>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="tenantId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentityUser?> FindByTenantIdAndUserNameAsync(
        [NotNull] string userName,
        Guid? tenantId,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .FirstOrDefaultAsync(
                u => u.TenantId == tenantId && u.UserName == userName,
                GetCancellationToken(cancellationToken)
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUser>> GetListByIdsAsync(IEnumerable<Guid> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceRoleId"></param>
    /// <param name="targetRoleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task UpdateRoleAsync(Guid sourceRoleId, Guid? targetRoleId, CancellationToken cancellationToken = default)
    {
        if (targetRoleId != null)
        {
            var users = await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == targetRoleId).Select(x => x.UserId).ToArrayAsync(cancellationToken: cancellationToken);
            await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == sourceRoleId && !users.Contains(x.UserId)).ExecuteUpdateAsync(t => t.SetProperty(e => e.RoleId, targetRoleId), GetCancellationToken(cancellationToken));
            await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == sourceRoleId).ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
        }
        else
        {
            await (await GetDbContextAsync()).Set<IdentityUserRole>().Where(x => x.RoleId == sourceRoleId).ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceOrganizationId"></param>
    /// <param name="targetOrganizationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task UpdateOrganizationAsync(Guid sourceOrganizationId, Guid? targetOrganizationId, CancellationToken cancellationToken = default)
    {
        if (targetOrganizationId != null)
        {
            var users = await (await GetDbContextAsync()).Set<IdentityUserOrganizationUnit>().Where(x => x.OrganizationUnitId == targetOrganizationId).Select(x => x.UserId).ToArrayAsync(cancellationToken: cancellationToken);
            await (await GetDbContextAsync()).Set<IdentityUserOrganizationUnit>().Where(x => x.OrganizationUnitId == sourceOrganizationId && !users.Contains(x.UserId)).ExecuteUpdateAsync(t => t.SetProperty(e => e.OrganizationUnitId, targetOrganizationId), GetCancellationToken(cancellationToken));
            await (await GetDbContextAsync()).Set<IdentityUserOrganizationUnit>().Where(x => x.OrganizationUnitId == sourceOrganizationId).ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
        }
        else
        {
            await (await GetDbContextAsync()).Set<IdentityUserOrganizationUnit>().Where(x => x.OrganizationUnitId == sourceOrganizationId).ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="roleId"></param>
    /// <param name="organizationUnitId"></param>
    /// <param name="userName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="isLockedOut"></param>
    /// <param name="notActive"></param>
    /// <param name="emailConfirmed"></param>
    /// <param name="isExternal"></param>
    /// <param name="maxCreationTime"></param>
    /// <param name="minCreationTime"></param>
    /// <param name="maxModifitionTime"></param>
    /// <param name="minModifitionTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<IQueryable<IdentityUser>> GetFilteredQueryableAsync(
        string? filter = null,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        string? userName = null,
        string? phoneNumber = null,
        string? emailAddress = null,
        string? name = null,
        string? surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default)
    {
        var upperFilter = filter?.ToUpperInvariant();
        var query = await GetQueryableAsync();

        if (roleId.HasValue)
        {
            var dbContext = await GetDbContextAsync();
            var organizationUnitIds = await dbContext.Set<OrganizationUnitRole>().Where(q => q.RoleId == roleId.Value).Select(q => q.OrganizationUnitId).ToArrayAsync(cancellationToken: cancellationToken);
            query = query.Where(identityUser => identityUser.Roles.Any(x => x.RoleId == roleId.Value) || identityUser.OrganizationUnits.Any(x => organizationUnitIds.Contains(x.OrganizationUnitId)));
        }

        return query
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.NormalizedUserName.Contains(upperFilter!) ||
                    u.NormalizedEmail.Contains(upperFilter!) ||
                    (u.Name != null && u.Name.Contains(filter!)) ||
                    (u.Surname != null && u.Surname.Contains(filter!)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter!))
            )
            .WhereIf(organizationUnitId.HasValue, identityUser => identityUser.OrganizationUnits.Any(x => x.OrganizationUnitId == organizationUnitId!.Value))
            .WhereIf(!string.IsNullOrWhiteSpace(userName), x => x.UserName == userName)
            .WhereIf(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber)
            .WhereIf(!string.IsNullOrWhiteSpace(emailAddress), x => x.Email == emailAddress)
            .WhereIf(!string.IsNullOrWhiteSpace(name), x => x.Name == name)
            .WhereIf(!string.IsNullOrWhiteSpace(surname), x => x.Surname == surname)
            .WhereIf(isLockedOut.HasValue, x => (x.LockoutEnabled && x.LockoutEnd.HasValue && x.LockoutEnd.Value.CompareTo(DateTime.UtcNow) > 0) == isLockedOut!.Value)
            .WhereIf(notActive.HasValue, x => x.IsActive == !notActive!.Value)
            .WhereIf(emailConfirmed.HasValue, x => x.EmailConfirmed == emailConfirmed!.Value)
            .WhereIf(isExternal.HasValue, x => x.IsExternal == isExternal!.Value)
            .WhereIf(maxCreationTime != null, p => p.CreationTime <= maxCreationTime)
            .WhereIf(minCreationTime != null, p => p.CreationTime >= minCreationTime)
            .WhereIf(maxModifitionTime != null, p => p.LastModificationTime <= maxModifitionTime)
            .WhereIf(minModifitionTime != null, p => p.LastModificationTime >= minModifitionTime);
    }
}
