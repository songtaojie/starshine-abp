using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Starshine.Abp.Identity;

/// <summary>
/// 表示指定用户和角色类型的持久性存储的新实例。
/// </summary>
public class IdentityUserStore :
    IUserLoginStore<IdentityUser>,
    IUserRoleStore<IdentityUser>,
    IUserClaimStore<IdentityUser>,
    IUserPasswordStore<IdentityUser>,
    IUserSecurityStampStore<IdentityUser>,
    IUserEmailStore<IdentityUser>,
    IUserLockoutStore<IdentityUser>,
    IUserPhoneNumberStore<IdentityUser>,
    IUserTwoFactorStore<IdentityUser>,
    IUserAuthenticationTokenStore<IdentityUser>,
    IUserAuthenticatorKeyStore<IdentityUser>,
    IUserTwoFactorRecoveryCodeStore<IdentityUser>,
    ITransientDependency
{
    private const string InternalLoginProvider = "[AspNetUserStore]";
    private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
    private const string RecoveryCodeTokenName = "RecoveryCodes";

    /// <summary>
    /// 获取或设置当前操作发生的任何错误的 <see cref="IdentityErrorDescriber"/>。
    /// </summary>
    public IdentityErrorDescriber ErrorDescriber { get; set; }

    /// <summary>
    /// 获取或设置一个标志，指示在调用 CreateAsync、UpdateAsync 和 DeleteAsync 后是否应保留更改。
    /// </summary>
    /// <value>
    /// 如果应自动保留更改，则为 True，否则为 false。
    /// </value>
    public bool AutoSaveChanges { get; set; } = true;
    /// <summary>
    /// 角色存储库
    /// </summary>
    protected IIdentityRoleRepository RoleRepository { get; }
    /// <summary>
    /// Guid生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }
    /// <summary>
    /// 日志记录
    /// </summary>
    protected ILogger<IdentityRoleStore> Logger { get; }
    /// <summary>
    /// 查找规范器
    /// </summary>
    protected ILookupNormalizer LookupNormalizer { get; }
    /// <summary>
    /// 用户存储库
    /// </summary>
    protected IIdentityUserRepository UserRepository { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="roleRepository"></param>
    /// <param name="guidGenerator"></param>
    /// <param name="logger"></param>
    /// <param name="lookupNormalizer"></param>
    /// <param name="describer"></param>
    public IdentityUserStore(
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IGuidGenerator guidGenerator,
        ILogger<IdentityRoleStore> logger,
        ILookupNormalizer lookupNormalizer,
        IdentityErrorDescriber? describer = null)
    {
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        GuidGenerator = guidGenerator;
        Logger = logger;
        LookupNormalizer = lookupNormalizer;

        ErrorDescriber = describer ?? new IdentityErrorDescriber();
    }

    /// <summary>
    /// 获取指定 <paramref name="user"/> 的用户标识符。
    /// </summary>
    /// <param name="user">应检索其标识符的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>The <see cref="Task"/>表示异步操作，包含指定 <paramref name="user"/> 的标识符。</returns>
    public virtual Task<string> GetUserIdAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult(user.Id.ToString());
    }

    /// <summary>
    /// 获取指定 <paramref name="user"/> 的用户名。
    /// </summary>
    /// <param name="user">应检索其姓名的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>The <see cref="Task"/> 代表异步操作，包含指定 <paramref name="user"/> 的名称。</returns>
    public virtual Task<string?> GetUserNameAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult<string?>(user.UserName);
    }

    /// <summary>
    /// 为指定的 <paramref name="user"/> 设置给定的 <paramref name="userName" />。
    /// </summary>
    /// <param name="user">应设置其名称的用户。</param>
    /// <param name="userName">The user name to set.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual Task SetUserNameAsync([NotNull] IdentityUser user, string? userName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(userName, nameof(userName));
        user.UserName = userName;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取指定 <paramref name="user"/> 的规范化用户名。
    /// </summary>
    /// <param name="user">应检索其规范化名称的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>，包含指定 <paramref name="user"/> 的规范化用户名。</returns>
    public virtual Task<string?> GetNormalizedUserNameAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult<string?>(user.NormalizedUserName);
    }

    /// <summary>
    /// 为指定的 <paramref name="user"/> 设置给定的规范化名称。
    /// </summary>
    /// <param name="user">应设置其名称的用户。</param>
    /// <param name="normalizedName">要设置的规范化名称。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual Task SetNormalizedUserNameAsync([NotNull] IdentityUser user, string? normalizedName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(normalizedName, nameof(normalizedName));
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 在用户存储中创建指定的 <paramref name="user"/>。
    /// </summary>
    /// <param name="user">要创建的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>The <see cref="Task"/> 表示异步操作，包含创建操作的<see cref="IdentityResult"/>。</returns>
    public virtual async Task<IdentityResult> CreateAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        await UserRepository.InsertAsync(user, AutoSaveChanges, cancellationToken);
        return IdentityResult.Success;
    }

    /// <summary>
    /// 更新用户存储中指定的 <paramref name="user"/>。
    /// </summary>
    /// <param name="user">要更新的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的<see cref="Task"/>，包含更新操作的<see cref="IdentityResult"/>。</returns>
    public virtual async Task<IdentityResult> UpdateAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        try
        {
            await UserRepository.UpdateAsync(user, AutoSaveChanges, cancellationToken);
        }
        catch (AbpDbConcurrencyException ex)
        {
            Logger.LogError(ex, ex.Message); //Trigger some AbpHandledException event
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// 从用户存储中删除指定的 <paramref name="user"/>。
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的<see cref="Task"/>，包含更新操作的<see cref="IdentityResult"/>。</returns>
    public virtual async Task<IdentityResult> DeleteAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        try
        {
            await UserRepository.DeleteAsync(user, AutoSaveChanges, cancellationToken);
        }
        catch (AbpDbConcurrencyException ex)
        {
            Logger.LogError(ex, ex.Message); //Trigger some AbpHandledException event
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// 查找并返回具有指定 <paramref name="userId"/> 的用户（如果有）。
    /// </summary>
    /// <param name="userId">要搜索的用户 ID。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 表示异步操作的<see cref="Task"/>，包含与指定<paramref name="userId"/>匹配的用户（如果存在）。
    /// </returns>
    public virtual Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return UserRepository.FindAsync(Guid.Parse(userId), cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 查找并返回具有指定规范化用户名的用户（如果有）。
    /// </summary>
    /// <param name="normalizedUserName">要搜索的规范化用户名。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    ///表示异步操作的 <see cref="Task"/>，包含与指定 <paramref name="normalizedUserName"/> 匹配的用户（如果存在）。
    /// </returns>
    public virtual Task<IdentityUser?> FindByNameAsync([NotNull] string normalizedUserName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(normalizedUserName, nameof(normalizedUserName));
        return UserRepository.FindByNormalizedUserNameAsync(normalizedUserName, includeDetails: false, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 为用户设置密码哈希。
    /// </summary>
    /// <param name="user">为其设置密码哈希的用户。</param>
    /// <param name="passwordHash">要设置的密码哈希。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual Task SetPasswordHashAsync([NotNull] IdentityUser user, string? passwordHash, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(passwordHash, nameof(passwordHash));
        user.PasswordHash = passwordHash;
        user.SetLastPasswordChangeTime(DateTime.UtcNow);
        return Task.CompletedTask;
    }

    /// <summary>
    ///获取用户的密码哈希。
    /// </summary>
    /// <param name="user">要检索密码哈希的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>包含用户密码哈希值的 <see cref="Task{TResult}"/>。</returns>
    public virtual Task<string?> GetPasswordHashAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult<string?>(user.PasswordHash);
    }

    /// <summary>
    ///返回一个标志，指示指定的用户是否有密码。
    /// </summary>
    /// <param name="user">要检索密码哈希的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns><see cref="Task{TResult}"/> 包含一个标志，指示指定用户是否有密码。如果
    /// 用户有密码，则返回值为 true，否则为 false。</returns>
    public virtual Task<bool> HasPasswordAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult(user.PasswordHash != null);
    }

    /// <summary>
    /// 将给定的 <paramref name="normalizedRoleName"/> 添加到指定的 <paramref name="user"/>。
    /// </summary>
    /// <param name="user">要添加角色的用户。</param>
    /// <param name="normalizedRoleName">要添加的角色。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task AddToRoleAsync([NotNull] IdentityUser user, [NotNull] string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(normalizedRoleName, nameof(normalizedRoleName));
        if (await IsInRoleAsync(user, normalizedRoleName, cancellationToken))
        {
            return;
        }
        var role = await RoleRepository.FindByNormalizedNameAsync(normalizedRoleName, cancellationToken: cancellationToken);
        if (role == null)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "角色 {0} 不存在！", normalizedRoleName));
        }
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, cancellationToken);
        user.AddRole(role.Id);
    }

    /// <summary>
    /// 从指定的 <paramref name="user"/> 中删除给定的 <paramref name="normalizedRoleName"/>。
    /// </summary>
    /// <param name="user">要删除其角色的用户。</param>
    /// <param name="normalizedRoleName">要删除的角色。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task RemoveFromRoleAsync([NotNull] IdentityUser user, [NotNull] string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNullOrWhiteSpace(normalizedRoleName, nameof(normalizedRoleName));
        var role = await RoleRepository.FindByNormalizedNameAsync(normalizedRoleName, cancellationToken: cancellationToken);
        if (role == null)
        {
            return;
        }
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, cancellationToken);
        user.RemoveRole(role.Id);
    }

    /// <summary>
    /// 检索指定 <paramref name="user"/> 所属的角色。
    /// </summary>
    /// <param name="user">应检索其角色的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns><see cref="Task{TResult}"/> 包含用户所属的角色。</returns>
    public virtual async Task<IList<string>> GetRolesAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        var userRoles = await UserRepository.GetRoleNamesAsync(user.Id, cancellationToken: cancellationToken);
        var userOrganizationUnitRoles = await UserRepository.GetRoleNamesInOrganizationUnitAsync(user.Id, cancellationToken: cancellationToken);
        return userRoles.Union(userOrganizationUnitRoles).ToList();
    }

    /// <summary>
    ///返回一个标志，指示指定用户是否是给定 <paramref name="normalizedRoleName"/> 的成员。
    /// </summary>
    /// <param name="user">应检查其角色成员资格的用户。</param>
    /// <param name="normalizedRoleName">检查成员资格的角色</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns><see cref="Task{TResult}"/> 包含一个标志，指示指定用户是否是给定组的成员。如果
    /// 用户是该组的成员，则返回值为 true，否则为 false。</returns>
    public virtual async Task<bool> IsInRoleAsync([NotNull] IdentityUser user,[NotNull] string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNullOrWhiteSpace(normalizedRoleName, nameof(normalizedRoleName));
        var roles = await GetRolesAsync(user, cancellationToken);
        return roles
            .Select(r => LookupNormalizer.NormalizeName(r))
            .Contains(normalizedRoleName);
    }

    /// <summary>
    ///作为异步操作获取与指定 <paramref name="user"/> 关联的声明。
    /// </summary>
    /// <param name="user">应检索其声明的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>包含授予用户的声明的 <see cref="Task{TResult}"/>。</returns>
    public virtual async Task<IList<Claim>> GetClaimsAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);
        return user.Claims.Select(c => c.ToClaim()).ToList();
    }

    /// <summary>
    /// 添加给予指定 <paramref name="user"/> 的 <paramref name="claims"/>。
    /// </summary>
    /// <param name="user">要添加声明的用户。</param>
    /// <param name="claims">向用户添加的声明。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task AddClaimsAsync([NotNull] IdentityUser user, [NotNull] IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(claims, nameof(claims));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);
        user.AddClaims(GuidGenerator, claims);
    }

    /// <summary>
    /// 将指定 <paramref name="user"/> 上的 <paramref name="claim"/> 替换为 <paramref name="newClaim"/>。
    /// </summary>
    /// <param name="user">要替换声明的用户。</param>
    /// <param name="claim">要替换的声明。</param>
    /// <param name="newClaim">新的声明取代了 <paramref name="claim"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task ReplaceClaimAsync([NotNull] IdentityUser user, [NotNull] Claim claim, [NotNull] Claim newClaim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(claim, nameof(claim));
        Check.NotNull(newClaim, nameof(newClaim));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);
        user.ReplaceClaim(claim, newClaim);
    }

    /// <summary>
    /// 从指定的 <paramref name="user"/> 中删除给出的 <paramref name="claims"/>。
    /// </summary>
    /// <param name="user">要删除声明的用户。</param>
    /// <param name="claims">要求删除的声明。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task RemoveClaimsAsync([NotNull] IdentityUser user, [NotNull] IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(claims, nameof(claims));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);
        user.RemoveClaims(claims);
    }

    /// <summary>
    /// 添加给定 <paramref name="login"/> 给指定的 <paramref name="user"/>。
    /// </summary>
    /// <param name="user">要添加登录的用户。</param>
    /// <param name="login">要添加到用户的登录名。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task AddLoginAsync([NotNull] IdentityUser user, [NotNull] UserLoginInfo login, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(login, nameof(login));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins, cancellationToken);
        user.AddLogin(login);
    }

    /// <summary>
    /// 从指定的 <paramref name="user"/> 中删除给出的 <paramref name="loginProvider"/>。
    /// </summary>
    /// <param name="user">要删除的用户登录。</param>
    /// <param name="loginProvider">要从用户中删除的登录名。</param>
    /// <param name="providerKey"><paramref name="loginProvider"/> 提供的用于识别用户的密钥。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task RemoveLoginAsync([NotNull] IdentityUser user, [NotNull] string loginProvider, [NotNull] string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins, cancellationToken);
        user.RemoveLogin(loginProvider, providerKey);
    }

    /// <summary>
    /// 检索指定 <param ref="user"/> 的相关登录信息。
    /// </summary>
    /// <param name="user">要检索其关联登录信息的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 异步操作的 <see cref="Task"/>，包含指定 <paramref name="user"/> 的 <see cref="UserLoginInfo"/> 列表（如果有）。
    /// </returns>
    public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins, cancellationToken);
        return user.Logins.Select(l => l.ToUserLoginInfo()).ToList();
    }

    /// <summary>
    /// 检索与指定登录提供程序和登录提供程序密钥关联的用户。
    /// </summary>
    /// <param name="loginProvider">提供 <paramref name="providerKey"/> 的登录提供商。</param>
    /// <param name="providerKey"><paramref name="loginProvider"/> 提供的用于识别用户的密钥。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 异步操作的 <see cref="Task"/>，包含与指定登录提供程序和密钥匹配的用户（如果有）。
    /// </returns>
    public virtual Task<IdentityUser?> FindByLoginAsync([NotNull] string loginProvider, [NotNull] string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));
        return UserRepository.FindByLoginAsync(loginProvider, providerKey, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 获取一个标志，指示指定 <paramref name="user"/> 的电子邮件地址是否已经验证，如果电子邮件地址已经验证则为 true，否则为 false。
    /// </summary>
    /// <param name="user">应返回其电子邮件确认状态的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 包含异步操作结果的任务对象，一个标志，指示指定 <paramref name="user"/> 的电子邮件地址是否已经确认。
    /// </returns>
    public virtual Task<bool> GetEmailConfirmedAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult(user.EmailConfirmed);
    }

    /// <summary>
    /// 设置标志，指??示指定的<paramref name="user"/>的电子邮件地址是否已经确认。
    /// </summary>
    /// <param name="user">应设置其电子邮件确认状态的用户。</param>
    /// <param name="confirmed">A flag indicating if the email address has been confirmed, true if the address is confirmed otherwise false.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的任务对象。</returns>
    public virtual Task SetEmailConfirmedAsync([NotNull] IdentityUser user, bool confirmed, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        user.SetEmailConfirmed(confirmed);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 为 <paramref name="user"/> 设置 <paramref name="email"/> 地址。
    /// </summary>
    /// <param name="user">应设置其电子邮件的用户。</param>
    /// <param name="email">要设置的电子邮件。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的任务对象。</returns>
    public virtual Task SetEmailAsync([NotNull] IdentityUser user, [NotNull] string? email, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(email, nameof(email));
        user.Email = email;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取指定 <paramref name="user"/> 的电子邮件地址。
    /// </summary>
    /// <param name="user">应返回其电子邮件的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>包含异步操作结果的任务对象，为指定的<paramref name="user"/>提供邮件地址。</returns>
    public virtual Task<string?> GetEmailAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult<string?>(user.Email);
    }

    /// <summary>
    /// 返回指定 <paramref name="user"/> 的规范化电子邮件。
    /// </summary>
    /// <param name="user">要检索其电子邮件地址的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 包含异步查找操作的结果的任务对象，以及与指定用户关联的规范化电子邮件地址（如果有）。
    /// </returns>
    public virtual Task<string?> GetNormalizedEmailAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult<string?>(user.NormalizedEmail);
    }

    /// <summary>
    /// 为指定的 <paramref name="user"/> 设置规范化的电子邮件。
    /// </summary>
    /// <param name="user">要设置电子邮件地址的用户。</param>
    /// <param name="normalizedEmail">为指定 <paramref name="user"/> 设置的规范化电子邮件。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的任务对象。</returns>
    public virtual Task SetNormalizedEmailAsync([NotNull] IdentityUser user, string? normalizedEmail, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取与指定的规范化电子邮件地址关联的用户（如果有）。
    /// </summary>
    /// <param name="normalizedEmail">返回用户的规范化电子邮件地址。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 包含异步查找操作结果的任务对象，如果有与指定的规范化电子邮件地址相关联的用户。
    /// </returns>
    public virtual Task<IdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return UserRepository.FindByNormalizedEmailAsync(normalizedEmail, includeDetails: false, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 获取用户上次锁定到期的最后一个 <see cref="DateTimeOffset"/>（如果有）。过去的任何时间都应表示用户未被锁定。
    /// </summary>
    /// <param name="user">应检索其锁定日期的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    ///<see cref="Task{TResult}"/> 表示异步查询的结果，<see cref="DateTimeOffset"/> 包含用户锁定上次过期的时间（如果有）。
    /// </returns>
    public virtual Task<DateTimeOffset?> GetLockoutEndDateAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult(user.LockoutEnd);
    }

    /// <summary>
    /// 锁定用户，直到指定的结束日期过去。设置过去的结束日期会立即解锁用户。
    /// </summary>
    /// <param name="user">应设置锁定日期的用户。</param>
    /// <param name="lockoutEnd"><see cref="DateTimeOffset"/> 之后 <paramref name="user"/> 的锁定应该结束。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual Task SetLockoutEndDateAsync([NotNull] IdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        user.LockoutEnd = lockoutEnd;
        return Task.CompletedTask;
    }

    /// <summary>
    ///记录发生了失败的访问，并增加失败访问计数。
    /// </summary>
    /// <param name="user">应增加取消次数的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>，包含增加的失败访问计数。</returns>
    public virtual Task<int> IncrementAccessFailedCountAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        user.AccessFailedCount++;
        return Task.FromResult(user.AccessFailedCount);
    }

    /// <summary>
    /// 重置用户的访问失败次数。
    /// </summary>
    /// <param name="user">应重置访问失败次数的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    /// <remarks>这通常在成功访问帐户后调用。</remarks>
    public virtual Task ResetAccessFailedCountAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        user.AccessFailedCount = 0;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 检索指定 <paramref name="user"/> 的当前失败访问计数。
    /// </summary>
    /// <param name="user">应检索其访问失败次数的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>，包含失败的访问计数。</returns>
    public virtual Task<int> GetAccessFailedCountAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult(user.AccessFailedCount);
    }

    /// <summary>
    /// 检索一个标志，指示是否可以为指定用户启用用户锁定。
    /// </summary>
    /// <param name="user">应返回被锁定的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 代表异步操作的 <see cref="Task"/>，如果可以锁定用户则为 true，否则为 false。
    /// </returns>
    public virtual Task<bool> GetLockoutEnabledAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult(user.LockoutEnabled);
    }

    /// <summary>
    /// 设置标志指示是否可以锁定指定的 <paramref name="user"/>。
    /// </summary>
    /// <param name="user">应设置可锁定的用户。</param>
    /// <param name="enabled">一个标志，指示是否可以为指定的 <paramref name="user"/> 启用锁定。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual Task SetLockoutEnabledAsync([NotNull] IdentityUser user, bool enabled, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        user.LockoutEnabled = enabled;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 为指定的<paramref name="user"/>设置电话号码。
    /// </summary>
    /// <param name="user">需要设置电话号码的用户。</param>
    /// <param name="phoneNumber">要设置的电话号码。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual Task SetPhoneNumberAsync([NotNull] IdentityUser user, string? phoneNumber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(phoneNumber, nameof(phoneNumber));
        user.PhoneNumber = phoneNumber;
        return Task.CompletedTask;
    }

    /// <summary>
    ///获取指定 <paramref name="user"/> 的电话号码（如果有）。
    /// </summary>
    /// <param name="user">需要检索其电话号码的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>，包含用户的电话号码（如果有）。</returns>
    public virtual Task<string?> GetPhoneNumberAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult<string?>(user.PhoneNumber);
    }

    /// <summary>
    /// 获取一个标志，指示指定<paramref name="user"/>的电话号码是否已经确认。
    /// </summary>
    /// <param name="user">向用户返回一个标志，表明他们的电话号码是否已确认。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 代表异步操作的<see cref="Task"/>，如果指定的<paramref name="user"/>具有确认的电话号码则返回true，否则返回false。
    /// </returns>
    public virtual Task<bool> GetPhoneNumberConfirmedAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    /// <summary>
    /// 设置一个标志，指示指定的 <paramref name="user"/> 的电话号码是否已经确认。
    /// </summary>
    /// <param name="user">需要设置电话号码确认状态的用户。</param>
    /// <param name="confirmed">指示用户的电话号码是否已经确认的标志。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual Task SetPhoneNumberConfirmedAsync([NotNull] IdentityUser user, bool confirmed, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        user.SetPhoneNumberConfirmed(confirmed);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 为指定的 <paramref name="user"/> 设置提供的安全 <paramref name="stamp"/>。
    /// </summary>
    /// <param name="user">应设置安全戳记的用户。</param>
    /// <param name="stamp">要设置的安全印章。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual Task SetSecurityStampAsync([NotNull] IdentityUser user, string stamp, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        user.SecurityStamp = stamp;
        return Task.CompletedTask;
    }

    /// <summary>
    ///获取指定 <paramref name="user" /> 的安全印章。
    /// </summary>
    /// <param name="user">应设置安全戳记的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>，包含指定 <paramref name="user"/> 的安全戳。</returns>
    public virtual Task<string?> GetSecurityStampAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult<string?>(user.SecurityStamp);
    }

    /// <summary>
    /// 设置一个标志，指示指定的 <paramref name="user"/> 是否启用了双因素身份验证，作为异步操作。
    /// </summary>
    /// <param name="user">应设置双因素身份验证启用状态的用户。</param>
    /// <param name="enabled">一个标志，指示指定的 <paramref name="user"/> 是否启用了双因素身份验证。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual Task SetTwoFactorEnabledAsync([NotNull] IdentityUser user, bool enabled, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        user.TwoFactorEnabled = enabled;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 返回一个标志，指示指定的 <paramref name="user"/> 是否启用了双因素身份验证，作为异步操作。
    /// </summary>
    /// <param name="user">应设置双因素身份验证启用状态的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 代表异步操作的 <see cref="Task"/>，包含一个标志，指示指定的 <paramref name="user"/> 是否启用了双因素身份验证。
    /// </returns>
    public virtual Task<bool> GetTwoFactorEnabledAsync([NotNull] IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        return Task.FromResult(user.TwoFactorEnabled);
    }

    /// <summary>
    /// 检索具有指定声明的所有用户。
    /// </summary>
    /// <param name="claim">应检索其用户的声明。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// <see cref="Task"/> 包含包含指定声明的用户列表（如果有）。
    /// </returns>
    public virtual async Task<IList<IdentityUser>> GetUsersForClaimAsync([NotNull] Claim claim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(claim, nameof(claim));
        return await UserRepository.GetListByClaimAsync(claim, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 检索指定角色的所有用户。
    /// </summary>
    /// <param name="normalizedRoleName">应检索其用户的角色。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// <see cref="Task"/> 包含具有指定角色的用户列表（如果有）。
    /// </returns>
    public virtual async Task<IList<IdentityUser>> GetUsersInRoleAsync([NotNull] string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrEmpty(normalizedRoleName))
        {
            throw new ArgumentNullException(nameof(normalizedRoleName));
        }
        return await UserRepository.GetListByNormalizedRoleNameAsync(normalizedRoleName, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 为特定用户设置令牌值。
    /// </summary>
    /// <param name="user">用户。</param>
    /// <param name="loginProvider">令牌的身份验证提供商。</param>
    /// <param name="name">令牌的名称。</param>
    /// <param name="value">令牌的价值。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task SetTokenAsync([NotNull] IdentityUser user, string loginProvider, string name, string? value, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        Check.NotNull(value, nameof(value));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, cancellationToken);
        user.SetToken(loginProvider, name, value);
    }

    /// <summary>
    /// 删除用户的令牌。
    /// </summary>
    /// <param name="user">用户。</param>
    /// <param name="loginProvider">令牌的身份验证提供商。</param>
    /// <param name="name">令牌的名称。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task RemoveTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, cancellationToken);
        user.RemoveToken(loginProvider, name);
    }

    /// <summary>
    ///返回令牌值。
    /// </summary>
    /// <param name="user">用户。</param>
    /// <param name="loginProvider">令牌的身份验证提供商。</param>
    /// <param name="name">令牌的名称。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task<string?> GetTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(user, nameof(user));
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Tokens, cancellationToken);
        return user.FindToken(loginProvider, name)?.Value;
    }
    /// <summary>
    /// 设置身份验证器密钥
    /// </summary>
    /// <param name="user"></param>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task SetAuthenticatorKeyAsync(IdentityUser user, string key, CancellationToken cancellationToken = default)
    {
        return SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);
    }

    /// <summary>
    /// 获取身份验证器密钥
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task<string?> GetAuthenticatorKeyAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        return GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);
    }

    /// <summary>
    /// 返回对于用户来说还有多少恢复代码有效。
    /// </summary>
    /// <param name="user">拥有恢复代码的用户。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>用户的有效恢复代码的数量。</returns>
    public virtual async Task<int> CountCodesAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
        if (mergedCodes.Length > 0)
        {
            return mergedCodes.Split(';').Length;
        }

        return 0;
    }

    /// <summary>
    /// 更新用户的恢复代码，同时使所有先前的恢复代码无效。
    /// </summary>
    /// <param name="user">存储新恢复代码的用户。</param>
    /// <param name="recoveryCodes">用户的新恢复代码。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>用户的新恢复代码。</returns>
    public virtual Task ReplaceCodesAsync(IdentityUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken = default)
    {
        var mergedCodes = string.Join(";", recoveryCodes);
        return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
    }

    /// <summary>
    ///返回恢复代码对用户是否有效。注意：恢复代码仅有效一次，使用后将失效。
    /// </summary>
    /// <param name="user">拥有恢复代码的用户。</param>
    /// <param name="code">要使用的恢复代码。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>如果为用户找到恢复代码则为 True。</returns>
    public virtual async Task<bool> RedeemCodeAsync(IdentityUser user, string code, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));
        Check.NotNull(code, nameof(code));

        var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
        var splitCodes = mergedCodes.Split(';');
        if (splitCodes.Contains(code))
        {
            var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
            await ReplaceCodesAsync(user, updatedCodes, cancellationToken);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 内部登录提供者
    /// </summary>
    /// <returns></returns>
    public virtual Task<string> GetInternalLoginProviderAsync()
    {
        return Task.FromResult(InternalLoginProvider);
    }

    /// <summary>
    /// AuthenticatorKeyToken名称
    /// </summary>
    /// <returns></returns>
    public virtual Task<string> GetAuthenticatorKeyTokenNameAsync()
    {
        return Task.FromResult(AuthenticatorKeyTokenName);
    }

    /// <summary>
    /// 恢复代码令牌名称
    /// </summary>
    /// <returns></returns>
    public virtual Task<string> GetRecoveryCodeTokenNameAsync()
    {
        return Task.FromResult(RecoveryCodeTokenName);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Dispose()
    {

    }
}
