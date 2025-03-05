using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Starshine.Abp.Identity.Localization;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;
using Volo.Abp;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份角色管理器
/// </summary>
public class IdentityRoleManager : RoleManager<IdentityRole>, IDomainService
{
    /// <summary>
    /// 取消令牌
    /// </summary>
    protected override CancellationToken CancellationToken => CancellationTokenProvider.Token;
    /// <summary>
    /// 本地化
    /// </summary>
    protected IStringLocalizer<IdentityResource> Localizer { get; }
    /// <summary>
    /// CancellationToken 提供程序
    /// </summary>
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    /// <summary>
    /// 用户存储库
    /// </summary>
    protected IIdentityUserRepository UserRepository { get; }
    /// <summary>
    /// 组织单位存储库
    /// </summary>
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    /// <summary>
    /// 组织单位
    /// </summary>
    protected OrganizationUnitManager OrganizationUnitManager { get; }
    /// <summary>
    /// 动态声明缓存
    /// </summary>
    protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <param name="roleValidators"></param>
    /// <param name="keyNormalizer"></param>
    /// <param name="errors"></param>
    /// <param name="logger"></param>
    /// <param name="localizer"></param>
    /// <param name="cancellationTokenProvider"></param>
    /// <param name="userRepository"></param>
    /// <param name="organizationUnitRepository"></param>
    /// <param name="organizationUnitManager"></param>
    /// <param name="dynamicClaimCache"></param>
    public IdentityRoleManager(
        IdentityRoleStore store,
        IEnumerable<IRoleValidator<IdentityRole>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<IdentityRoleManager> logger,
        IStringLocalizer<IdentityResource> localizer,
        ICancellationTokenProvider cancellationTokenProvider,
        IIdentityUserRepository userRepository,
        IOrganizationUnitRepository organizationUnitRepository,
        OrganizationUnitManager organizationUnitManager,
        IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache)
        : base(
              store,
              roleValidators,
              keyNormalizer,
              errors,
              logger)
    {
        Localizer = localizer;
        CancellationTokenProvider = cancellationTokenProvider;
        UserRepository = userRepository;
        OrganizationUnitRepository = organizationUnitRepository;
        OrganizationUnitManager = organizationUnitManager;
        DynamicClaimCache = dynamicClaimCache;
    }

    /// <summary>
    /// 根据id获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public virtual async Task<IdentityRole> GetByIdAsync(Guid id)
    {
        var role = await Store.FindByIdAsync(id.ToString(), CancellationToken) ?? throw new EntityNotFoundException(typeof(IdentityRole), id);
        return role;
    }

    /// <summary>
    /// 设置角色姓名
    /// </summary>
    /// <param name="role"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public async override Task<IdentityResult> SetRoleNameAsync(IdentityRole role, string? name)
    {
        if (role.IsStatic && role.Name != name)
        {
            throw new BusinessException(IdentityErrorCodes.StaticRoleRenaming);
        }

        var userIdList = await UserRepository.GetUserIdListByRoleIdAsync(role.Id, cancellationToken: CancellationToken);
        var result = await base.SetRoleNameAsync(role, name);
        if (result.Succeeded)
        {
            Logger.LogDebug($"删除角色用户: {role.Id}的动态声明缓存");
            await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, role.TenantId)), token: CancellationToken);
        }

        return result;
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public async override Task<IdentityResult> DeleteAsync(IdentityRole role)
    {
        if (role.IsStatic)
        {
            throw new BusinessException(IdentityErrorCodes.StaticRoleDeletion);
        }

        var userIdList = await UserRepository.GetUserIdListByRoleIdAsync(role.Id, cancellationToken: CancellationToken);
        var orgList = await OrganizationUnitRepository.GetListByRoleIdAsync(role.Id, includeDetails: false, cancellationToken: CancellationToken);
        var result = await base.DeleteAsync(role);
        if (result.Succeeded)
        {
            Logger.LogDebug($"删除角色用户：{role.Id}的动态声明缓存");
            await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, role.TenantId)), token: CancellationToken);
            foreach (var organizationUnit in orgList)
            {
                await OrganizationUnitManager.RemoveDynamicClaimCacheAsync(organizationUnit);
            }
        }

        return result;
    }
}
