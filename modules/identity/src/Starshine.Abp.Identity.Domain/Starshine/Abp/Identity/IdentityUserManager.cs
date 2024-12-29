using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Starshine.Abp.Identity.Settings;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份用户管理器
/// </summary>
public class IdentityUserManager : UserManager<IdentityUser>, IDomainService
{
    /// <summary>
    /// 角色存储库
    /// </summary>
    protected IIdentityRoleRepository RoleRepository { get; }
    /// <summary>
    /// 用户存储库
    /// </summary>
    protected IIdentityUserRepository UserRepository { get; }
    /// <summary>
    /// 组织单位存储库
    /// </summary>
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    /// <summary>
    /// 设置提供者
    /// </summary>
    protected ISettingProvider SettingProvider { get; }
    /// <summary>
    /// CancellationToken 提供程序
    /// </summary>
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    /// <summary>
    /// 分布式事件总线
    /// </summary>
    protected IDistributedEventBus DistributedEventBus { get; }
    /// <summary>
    /// IdentityLink 用户存储库
    /// </summary>
    protected IIdentityLinkUserRepository IdentityLinkUserRepository { get; }
    /// <summary>
    /// 动态声明缓存
    /// </summary>
    protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; }
    /// <summary>
    /// 
    /// </summary>
    protected override CancellationToken CancellationToken => CancellationTokenProvider.Token;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <param name="roleRepository"></param>
    /// <param name="userRepository"></param>
    /// <param name="optionsAccessor"></param>
    /// <param name="passwordHasher"></param>
    /// <param name="userValidators"></param>
    /// <param name="passwordValidators"></param>
    /// <param name="keyNormalizer"></param>
    /// <param name="errors"></param>
    /// <param name="services"></param>
    /// <param name="logger"></param>
    /// <param name="cancellationTokenProvider"></param>
    /// <param name="organizationUnitRepository"></param>
    /// <param name="settingProvider"></param>
    /// <param name="distributedEventBus"></param>
    /// <param name="identityLinkUserRepository"></param>
    /// <param name="dynamicClaimCache"></param>
    public IdentityUserManager(
        IdentityUserStore store,
        IIdentityRoleRepository roleRepository,
        IIdentityUserRepository userRepository,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<IdentityUserManager> logger,
        ICancellationTokenProvider cancellationTokenProvider,
        IOrganizationUnitRepository organizationUnitRepository,
        ISettingProvider settingProvider,
        IDistributedEventBus distributedEventBus,
        IIdentityLinkUserRepository identityLinkUserRepository,
        IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache)
        : base(
            store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger)
    {
        OrganizationUnitRepository = organizationUnitRepository;
        SettingProvider = settingProvider;
        DistributedEventBus = distributedEventBus;
        RoleRepository = roleRepository;
        UserRepository = userRepository;
        IdentityLinkUserRepository = identityLinkUserRepository;
        DynamicClaimCache = dynamicClaimCache;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="validatePassword"></param>
    /// <returns></returns>
    public virtual async Task<IdentityResult> CreateAsync(IdentityUser user, string password, bool validatePassword)
    {
        var result = await UpdatePasswordHash(user, password, validatePassword);
        if (!result.Succeeded)
        {
            return result;
        }
        return await CreateAsync(user);
    }
    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async override Task<IdentityResult> DeleteAsync(IdentityUser user)
    {
        user.Claims.Clear();
        user.Roles.Clear();
        user.Tokens.Clear();
        user.Logins.Clear();
        user.OrganizationUnits.Clear();
        await IdentityLinkUserRepository.DeleteAsync(new IdentityLinkUserInfo(user.Id, user.TenantId), CancellationToken);
        await UpdateAsync(user);

        return await base.DeleteAsync(user);
    }
    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    protected async override Task<IdentityResult> UpdateUserAsync(IdentityUser user)
    {
        var result = await base.UpdateUserAsync(user);

        if (result.Succeeded)
        {
            await DynamicClaimCache.RemoveAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(user.Id, user.TenantId), token: CancellationToken);
        }
        return result;
    }
    /// <summary>
    /// 根据主键获取用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public virtual async Task<IdentityUser> GetByIdAsync(Guid id)
    {
        var user = await Store.FindByIdAsync(id.ToString(), CancellationToken) ?? throw new EntityNotFoundException(typeof(IdentityUser), id);
        return user;
    }
    /// <summary>
    /// 设置角色信息
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roleNames"></param>
    /// <returns></returns>
    public virtual async Task<IdentityResult> SetRolesAsync([NotNull] IdentityUser user,
        [NotNull] IEnumerable<string> roleNames)
    {
        Check.NotNull(user, nameof(user));
        Check.NotNull(roleNames, nameof(roleNames));

        var currentRoleNames = await GetRolesAsync(user);
        var result = await RemoveFromRolesAsync(user, currentRoleNames.Except(roleNames).Distinct());
        if (!result.Succeeded)
        {
            return result;
        }

        result = await AddToRolesAsync(user, roleNames.Except(currentRoleNames).Distinct());
        if (!result.Succeeded)
        {
            return result;
        }

        return IdentityResult.Success;
    }
    /// <summary>
    /// 是否是组织单元
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ouId"></param>
    /// <returns></returns>
    public virtual async Task<bool> IsInOrganizationUnitAsync(Guid userId, Guid ouId)
    {
        var user = await UserRepository.GetAsync(userId, cancellationToken: CancellationToken);
        return user.IsInOrganizationUnit(ouId);
    }
    /// <summary>
    /// 是否是组织单元
    /// </summary>
    /// <param name="user"></param>
    /// <param name="ou"></param>
    /// <returns></returns>
    public virtual async Task<bool> IsInOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
    {
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits,
            CancellationTokenProvider.Token);
        return user.IsInOrganizationUnit(ou.Id);
    }
    /// <summary>
    /// 添加组织单元
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ouId"></param>
    /// <returns></returns>
    public virtual async Task AddToOrganizationUnitAsync(Guid userId, Guid ouId)
    {
        await AddToOrganizationUnitAsync(
            await UserRepository.GetAsync(userId, cancellationToken: CancellationToken),
            await OrganizationUnitRepository.GetAsync(ouId, cancellationToken: CancellationToken)
        );
    }
    /// <summary>
    /// 添加用户到组织单元
    /// </summary>
    /// <param name="user"></param>
    /// <param name="ou"></param>
    /// <returns></returns>
    public virtual async Task AddToOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
    {
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits,
            CancellationTokenProvider.Token);

        if (user.OrganizationUnits.Any(cou => cou.OrganizationUnitId == ou.Id))
        {
            return;
        }

        await CheckMaxUserOrganizationUnitMembershipCountAsync(user.OrganizationUnits.Count + 1);

        user.AddOrganizationUnit(ou.Id);
        await UserRepository.UpdateAsync(user, cancellationToken: CancellationToken);

        await DynamicClaimCache.RemoveAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(user.Id, user.TenantId), token: CancellationToken);
    }
    /// <summary>
    /// 移除用户从组织单元
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ouId"></param>
    /// <returns></returns>
    public virtual async Task RemoveFromOrganizationUnitAsync(Guid userId, Guid ouId)
    {
        var user = await UserRepository.GetAsync(userId, cancellationToken: CancellationToken);
        user.RemoveOrganizationUnit(ouId);
        await UserRepository.UpdateAsync(user, cancellationToken: CancellationToken);

        await DynamicClaimCache.RemoveAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(user.Id, user.TenantId), token: CancellationToken);
    }

    /// <summary>
    /// 移除用户从组织单元
    /// </summary>
    /// <param name="user"></param>
    /// <param name="ou"></param>
    /// <returns></returns>
    public virtual async Task RemoveFromOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
    {
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits,
            CancellationTokenProvider.Token);

        user.RemoveOrganizationUnit(ou.Id);
        await UserRepository.UpdateAsync(user, cancellationToken: CancellationToken);
    }
    /// <summary>
    /// 设置用户到组织单元
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="organizationUnitIds"></param>
    /// <returns></returns>
    public virtual async Task SetOrganizationUnitsAsync(Guid userId, params Guid[] organizationUnitIds)
    {
        await SetOrganizationUnitsAsync(
            await UserRepository.GetAsync(userId, cancellationToken: CancellationToken),
            organizationUnitIds
        );
    }
    /// <summary>
    /// 设置用户到组织单元
    /// </summary>
    /// <param name="user"></param>
    /// <param name="organizationUnitIds"></param>
    /// <returns></returns>
    public virtual async Task SetOrganizationUnitsAsync(IdentityUser user, params Guid[] organizationUnitIds)
    {
        Check.NotNull(user, nameof(user));
        Check.NotNull(organizationUnitIds, nameof(organizationUnitIds));

        await CheckMaxUserOrganizationUnitMembershipCountAsync(organizationUnitIds.Length);

        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits,
            CancellationTokenProvider.Token);

        //从已删除的 OU 中删除
        foreach (var ouId in user.OrganizationUnits.Select(uou => uou.OrganizationUnitId).ToArray())
        {
            if (!organizationUnitIds.Contains(ouId))
            {
                user.RemoveOrganizationUnit(ouId);
            }
        }

        //Add to added OUs
        foreach (var organizationUnitId in organizationUnitIds)
        {
            if (!user.IsInOrganizationUnit(organizationUnitId))
            {
                user.AddOrganizationUnit(organizationUnitId);
            }
        }

        await UserRepository.UpdateAsync(user, cancellationToken: CancellationToken);
    }

    private async Task CheckMaxUserOrganizationUnitMembershipCountAsync(int requestedCount)
    {
        var maxCount =
            await SettingProvider.GetAsync<int>(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount);
        if (requestedCount > maxCount)
        {
            throw new BusinessException(IdentityErrorCodes.MaxAllowedOuMembership)
                .WithData("MaxUserMembershipCount", maxCount);
        }
    }
    /// <summary>
    /// 获取用户的组织单元
    /// </summary>
    /// <param name="user"></param>
    /// <param name="includeDetails"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(IdentityUser user, bool includeDetails = false)
    {
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits, CancellationTokenProvider.Token);

        return await OrganizationUnitRepository.GetListAsync(user.OrganizationUnits.Select(t => t.OrganizationUnitId), includeDetails, cancellationToken: CancellationToken);
    }
    /// <summary>
    /// 获取组织单元中的用户
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="includeChildren"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(OrganizationUnit organizationUnit, bool includeChildren = false)
    {
        if (includeChildren)
        {
            return await UserRepository.GetUsersInOrganizationUnitWithChildrenAsync(organizationUnit.Code, CancellationToken);
        }
        else
        {
            return await UserRepository.GetUsersInOrganizationUnitAsync(organizationUnit.Id, CancellationToken);
        }
    }
    /// <summary>
    /// 添加默认的角色
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public virtual async Task<IdentityResult> AddDefaultRolesAsync([NotNull] IdentityUser user)
    {
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, CancellationToken);

        foreach (var role in await RoleRepository.GetDefaultOnesAsync(cancellationToken: CancellationToken))
        {
            if (!user.IsInRole(role.Id))
            {
                user.AddRole(role.Id);
            }
        }

        return await UpdateUserAsync(user);
    }
    /// <summary>
    /// 应定期更改密码
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public virtual async Task<bool> ShouldPeriodicallyChangePasswordAsync(IdentityUser user)
    {
        Check.NotNull(user, nameof(user));

        if (user.PasswordHash.IsNullOrWhiteSpace())
        {
            return false;
        }

        var forceUsersToPeriodicallyChangePassword = await SettingProvider.GetAsync<bool>(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword);
        if (!forceUsersToPeriodicallyChangePassword)
        {
            return false;
        }

        var lastPasswordChangeTime = user.LastPasswordChangeTime ?? DateTime.SpecifyKind(user.CreationTime, DateTimeKind.Utc);
        var passwordChangePeriodDays = await SettingProvider.GetAsync<int>(IdentitySettingNames.Password.PasswordChangePeriodDays);

        return passwordChangePeriodDays > 0 && lastPasswordChangeTime.AddDays(passwordChangePeriodDays) < DateTime.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="AbpException"></exception>
    public virtual async Task ResetRecoveryCodesAsync(IdentityUser user)
    {
        if (!(Store is IdentityUserStore identityUserStore))
        {
            throw new AbpException($"Store is not an instance of {typeof(IdentityUserStore).AssemblyQualifiedName}");
        }

        await identityUserStore.SetTokenAsync(user, await identityUserStore.GetInternalLoginProviderAsync(), await identityUserStore.GetRecoveryCodeTokenNameAsync(), string.Empty, CancellationToken);
    }
    /// <summary>
    /// 设置邮箱
    /// </summary>
    /// <param name="user"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public async override Task<IdentityResult> SetEmailAsync(IdentityUser user, string? email)
    {
        var oldMail = user.Email;
        var result = await base.SetEmailAsync(user, email);
        result.CheckErrors();
        if (!string.IsNullOrEmpty(oldMail) && !oldMail.Equals(email, StringComparison.OrdinalIgnoreCase))
        {
            await DistributedEventBus.PublishAsync(
                new IdentityUserEmailChangedEto
                {
                    Id = user.Id,
                    TenantId = user.TenantId,
                    Email = email,
                    OldEmail = oldMail
                });
        }

        return result;
    }

    /// <summary>
    /// 设置用户名
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async override Task<IdentityResult> SetUserNameAsync(IdentityUser user, string? userName)
    {
        var oldUserName = user.UserName;
        var result = await base.SetUserNameAsync(user, userName);
        result.CheckErrors();
        if (!string.IsNullOrEmpty(oldUserName) && oldUserName != userName)
        {
            await DistributedEventBus.PublishAsync(
                new IdentityUserUserNameChangedEto
                {
                    Id = user.Id,
                    TenantId = user.TenantId,
                    UserName = userName,
                    OldUserName = oldUserName
                });
        }

        return result;
    }
    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="sourceRoleId"></param>
    /// <param name="targetRoleId"></param>
    /// <returns></returns>
    public virtual async Task UpdateRoleAsync(Guid sourceRoleId, Guid? targetRoleId)
    {
        var sourceRole = await RoleRepository.GetAsync(sourceRoleId, cancellationToken: CancellationToken);

        Logger.LogDebug($"删除角色为 {sourceRoleId} 的用户的动态声明缓存");
        var userIdList = await UserRepository.GetUserIdListByRoleIdAsync(sourceRoleId, cancellationToken: CancellationToken);
        await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, sourceRole.TenantId)), token: CancellationToken);
        var targetRole = targetRoleId.HasValue ? await RoleRepository.GetAsync(targetRoleId.Value, cancellationToken: CancellationToken) : null;
        if (targetRole != null)
        {
            Logger.LogDebug($"删除角色为 {targetRoleId} 的用户的动态声明缓存");
            userIdList = await UserRepository.GetUserIdListByRoleIdAsync(targetRole.Id, cancellationToken: CancellationToken);
            await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, targetRole.TenantId)), token: CancellationToken);
        }
        await UserRepository.UpdateRoleAsync(sourceRoleId, targetRoleId, CancellationToken);
    }

    /// <summary>
    /// 更新组织
    /// </summary>
    /// <param name="sourceOrganizationId"></param>
    /// <param name="targetOrganizationId"></param>
    /// <returns></returns>
    public virtual async Task UpdateOrganizationAsync(Guid sourceOrganizationId, Guid? targetOrganizationId)
    {
        var sourceOrganization = await OrganizationUnitRepository.GetAsync(sourceOrganizationId, cancellationToken: CancellationToken);

        Logger.LogDebug($"删除组织 {sourceOrganizationId} 用户的动态声明缓存");
        var userIdList = await OrganizationUnitRepository.GetMemberIdsAsync(sourceOrganizationId, cancellationToken: CancellationToken);
        await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, sourceOrganization.TenantId)), token: CancellationToken);
        var targetOrganization = targetOrganizationId.HasValue ? await OrganizationUnitRepository.GetAsync(targetOrganizationId.Value, cancellationToken: CancellationToken) : null;
        if (targetOrganization != null)
        {
            Logger.LogDebug($"删除组织用户的动态声明缓存：{targetOrganizationId}");
            userIdList = await OrganizationUnitRepository.GetMemberIdsAsync(targetOrganization.Id, cancellationToken: CancellationToken);
            await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, targetOrganization.TenantId)), token: CancellationToken);
        }

        await UserRepository.UpdateOrganizationAsync(sourceOrganizationId, targetOrganizationId, CancellationToken);
    }
    /// <summary>
    /// 验证用户名
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public virtual async Task<bool> ValidateUserNameAsync(string userName, Guid? userId = null)
    {
        if (string.IsNullOrWhiteSpace(userName)) return false;

        if (!string.IsNullOrEmpty(Options.User.AllowedUserNameCharacters) && userName.Any(c => !Options.User.AllowedUserNameCharacters.Contains(c)))
        {
            return false;
        }

        var owner = await FindByNameAsync(userName);
        if (owner != null && owner.Id != userId)
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// 获取一个随机的用户名
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public virtual Task<string> GetRandomUserNameAsync(int length)
    {
        var allowedUserNameCharacters = Options.User.AllowedUserNameCharacters;
        if (allowedUserNameCharacters.IsNullOrWhiteSpace())
        {
            allowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }

        var randomUserName = string.Empty;
        var random = new Random();
        while (randomUserName.Length < length)
        {
            randomUserName += allowedUserNameCharacters[random.Next(0, allowedUserNameCharacters.Length)];
        }

        return Task.FromResult(randomUserName);
    }
    /// <summary>
    /// 根据邮箱获取用户名
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <exception cref="StarshineIdentityResultException"></exception>
    public virtual async Task<string> GetUserNameFromEmailAsync(string email)
    {
        const int maxTryCount = 20;
        var tryCount = 0;

        var userName = email.Split('@')[0];

        if (await ValidateUserNameAsync(userName))
        {
            // The username is valid.
            return userName;
        }

        if (Options.User.AllowedUserNameCharacters.IsNullOrWhiteSpace())
        {
            // AllowedUserNameCharacters 未设置。因此，我们生成一个随机用户名。
            tryCount = 0;
            do
            {
                var randomUserName = userName + RandomHelper.GetRandom(1000, 9999);
                if (await ValidateUserNameAsync(randomUserName))
                {
                    return randomUserName;
                }
                tryCount++;
            } while (tryCount < maxTryCount);
        }
        else if (!userName.All(Options.User.AllowedUserNameCharacters.Contains))
        {
            // 用户名包含不允许的字符。因此，我们将生成一个随机用户名。
            do
            {
                var randomUserName = await GetRandomUserNameAsync(userName.Length);
                if (await ValidateUserNameAsync(randomUserName))
                {
                    return randomUserName;
                }
                tryCount++;
            } while (tryCount < maxTryCount);
        }
        else if (Options.User.AllowedUserNameCharacters.Where(char.IsDigit).Distinct().Count() >= 4)
        {
            // The AllowedUserNameCharacters includes 4 numbers. So, we are generating 4 random numbers and appending to the username.
            var numbers = Options.User.AllowedUserNameCharacters.Where(char.IsDigit).OrderBy(x => Guid.NewGuid()).Take(4).ToArray();
            var minArray = numbers.OrderBy(x => x).ToArray();
            if (minArray[0] == '0')
            {
                var secondItem = minArray[1];
                minArray[0] = secondItem;
                minArray[1] = '0';
            }
            var min = int.Parse(new string(minArray));
            var max = int.Parse(new string(numbers.OrderByDescending(x => x).ToArray()));
            tryCount = 0;
            do
            {
                var randomUserName = userName + RandomHelper.GetRandom(min, max);
                if (await ValidateUserNameAsync(randomUserName))
                {
                    return randomUserName;
                }
                tryCount++;
            } while (tryCount < maxTryCount);
        }
        else
        {
            tryCount = 0;
            do
            {
                // The AllowedUserNameCharacters does not include numbers. So, we are generating 4 random characters and appending to the username.
                var randomUserName = userName + await GetRandomUserNameAsync(4);
                if (await ValidateUserNameAsync(randomUserName))
                {
                    return randomUserName;
                }
                tryCount++;
            } while (tryCount < maxTryCount);
        }

        Logger.LogError($"无法获取给定电子邮件地址的有效用户名：{email}，允许的字符：{Options.User.AllowedUserNameCharacters}，已尝试 {maxTryCount} 次。");
        throw new StarshineIdentityResultException(IdentityResult.Failed(new IdentityErrorDescriber().InvalidUserName(userName)));
    }
}
