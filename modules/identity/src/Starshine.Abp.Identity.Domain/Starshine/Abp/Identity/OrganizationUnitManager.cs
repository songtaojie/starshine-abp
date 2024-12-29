using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Starshine.Abp.Identity.Localization;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp;

namespace Starshine.Abp.Identity;

/// <summary>
///为组织单位执行域逻辑。
/// </summary>
public class OrganizationUnitManager : DomainService
{
    /// <summary>
    /// 组织单位存储库
    /// </summary>
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    /// <summary>
    /// 本地化
    /// </summary>
    protected IStringLocalizer<IdentityResource> Localizer { get; }
    /// <summary>
    /// IdentityRole 存储库
    /// </summary>
    protected IIdentityRoleRepository IdentityRoleRepository { get; }
    /// <summary>
    /// 动态声明缓存
    /// </summary>
    protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; }
    /// <summary>
    /// CancellationToken 提供程序
    /// </summary>
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnitRepository"></param>
    /// <param name="localizer"></param>
    /// <param name="identityRoleRepository"></param>
    /// <param name="dynamicClaimCache"></param>
    /// <param name="cancellationTokenProvider"></param>
    public OrganizationUnitManager(
        IOrganizationUnitRepository organizationUnitRepository,
        IStringLocalizer<IdentityResource> localizer,
        IIdentityRoleRepository identityRoleRepository,
        IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        OrganizationUnitRepository = organizationUnitRepository;
        Localizer = localizer;
        IdentityRoleRepository = identityRoleRepository;
        DynamicClaimCache = dynamicClaimCache;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task CreateAsync(OrganizationUnit organizationUnit)
    {
        organizationUnit.Code = await GetNextChildCodeAsync(organizationUnit.ParentId);
        await ValidateOrganizationUnitAsync(organizationUnit);
        await OrganizationUnitRepository.InsertAsync(organizationUnit);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(OrganizationUnit organizationUnit)
    {
        await ValidateOrganizationUnitAsync(organizationUnit);
        await OrganizationUnitRepository.UpdateAsync(organizationUnit);
        await RemoveDynamicClaimCacheAsync(organizationUnit);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
    {
        var lastChild = await GetLastChildOrNullAsync(parentId);
        if (lastChild != null)
        {
            return OrganizationUnit.CalculateNextCode(lastChild.Code);
        }

        var parentCode = parentId != null
            ? await GetCodeOrDefaultAsync(parentId.Value)
            : null;

        return OrganizationUnit.AppendCode(
            parentCode,
            OrganizationUnit.CreateCode(1)
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public virtual async Task<OrganizationUnit?> GetLastChildOrNullAsync(Guid? parentId)
    {
        var children = await OrganizationUnitRepository.GetChildrenAsync(parentId);
        return children.OrderBy(c => c.Code).LastOrDefault();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task DeleteAsync(Guid id)
    {
        var children = await FindChildrenAsync(id, true);

        foreach (var child in children)
        {
            await RemoveDynamicClaimCacheAsync(child);
            await OrganizationUnitRepository.RemoveAllMembersAsync(child);
            await OrganizationUnitRepository.RemoveAllRolesAsync(child);
            await OrganizationUnitRepository.DeleteAsync(child);
        }

        var organizationUnit = await OrganizationUnitRepository.GetAsync(id);

        await RemoveDynamicClaimCacheAsync(organizationUnit);
        await OrganizationUnitRepository.RemoveAllMembersAsync(organizationUnit);
        await OrganizationUnitRepository.RemoveAllRolesAsync(organizationUnit);
        await OrganizationUnitRepository.DeleteAsync(id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task MoveAsync(Guid id, Guid? parentId)
    {
        var organizationUnit = await OrganizationUnitRepository.GetAsync(id);
        if (organizationUnit.ParentId == parentId)
        {
            return;
        }

        //Should find children before Code change
        var children = await FindChildrenAsync(id, true);

        //Store old code of OU
        var oldCode = organizationUnit.Code;

        //Move OU
        organizationUnit.Code = await GetNextChildCodeAsync(parentId);
        organizationUnit.ParentId = parentId;

        await ValidateOrganizationUnitAsync(organizationUnit);

        //Update Children Codes
        foreach (var child in children)
        {
            child.Code = OrganizationUnit.AppendCode(organizationUnit.Code, OrganizationUnit.GetRelativeCode(child.Code, oldCode));
            await OrganizationUnitRepository.UpdateAsync(child);
        }

        await OrganizationUnitRepository.UpdateAsync(organizationUnit);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<string?> GetCodeOrDefaultAsync(Guid id)
    {
        var ou = await OrganizationUnitRepository.FindAsync(id);
        return ou?.Code;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <returns></returns>
    protected virtual async Task ValidateOrganizationUnitAsync(OrganizationUnit organizationUnit)
    {
        var siblings = (await FindChildrenAsync(organizationUnit.ParentId))
            .Where(ou => ou.Id != organizationUnit.Id)
            .ToList();

        if (siblings.Any(ou => ou.DisplayName == organizationUnit.DisplayName))
        {
            throw new BusinessException(IdentityErrorCodes.DuplicateOrganizationUnitDisplayName)
                .WithData("0", organizationUnit.DisplayName ?? string.Empty);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public async Task<List<OrganizationUnit>> FindChildrenAsync(Guid? parentId, bool recursive = false)
    {
        if (!recursive)
        {
            return await OrganizationUnitRepository.GetChildrenAsync(parentId, includeDetails: true);
        }

        if (!parentId.HasValue)
        {
            return await OrganizationUnitRepository.GetListAsync(includeDetails: true);
        }

        var code = await GetCodeOrDefaultAsync(parentId.Value);

        return await OrganizationUnitRepository.GetAllChildrenWithParentCodeAsync(code, parentId, includeDetails: true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="ou"></param>
    /// <returns></returns>
    public virtual Task<bool> IsInOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
    {
        return Task.FromResult(user.IsInOrganizationUnit(ou.Id));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="ouId"></param>
    /// <returns></returns>
    public virtual async Task AddRoleToOrganizationUnitAsync(Guid roleId, Guid ouId)
    {
        await AddRoleToOrganizationUnitAsync(
            await IdentityRoleRepository.GetAsync(roleId),
            await OrganizationUnitRepository.GetAsync(ouId, true)
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="role"></param>
    /// <param name="ou"></param>
    /// <returns></returns>
    public virtual async Task AddRoleToOrganizationUnitAsync(IdentityRole role, OrganizationUnit ou)
    {
        var currentRoles = ou.Roles;

        if (currentRoles.Any(r => r.OrganizationUnitId == ou.Id && r.RoleId == role.Id))
        {
            return;
        }
        ou.AddRole(role.Id);
        await OrganizationUnitRepository.UpdateAsync(ou);
        await RemoveDynamicClaimCacheAsync(ou);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="ouId"></param>
    /// <returns></returns>
    public virtual async Task RemoveRoleFromOrganizationUnitAsync(Guid roleId, Guid ouId)
    {
        await RemoveRoleFromOrganizationUnitAsync(
            await IdentityRoleRepository.GetAsync(roleId),
            await OrganizationUnitRepository.GetAsync(ouId, true)
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="role"></param>
    /// <param name="organizationUnit"></param>
    /// <returns></returns>
    public virtual async Task RemoveRoleFromOrganizationUnitAsync(IdentityRole role, OrganizationUnit organizationUnit)
    {
        organizationUnit.RemoveRole(role.Id);
        await OrganizationUnitRepository.UpdateAsync(organizationUnit);
        await RemoveDynamicClaimCacheAsync(organizationUnit);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <returns></returns>
    public virtual async Task RemoveDynamicClaimCacheAsync(OrganizationUnit organizationUnit)
    {
        Logger.LogDebug($"Remove dynamic claims cache for users of organization: {organizationUnit.Id}");
        var userIds = await OrganizationUnitRepository.GetMemberIdsAsync(organizationUnit.Id);
        await DynamicClaimCache.RemoveManyAsync(userIds.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, organizationUnit.TenantId)));
    }
}
