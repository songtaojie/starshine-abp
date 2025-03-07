using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Starshine.Abp.Identity.Managers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 外部登录提供程序基础
/// </summary>
public abstract class ExternalLoginProviderBase : IExternalLoginProvider
{
    /// <summary>
    /// Guid生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }
    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }
    /// <summary>
    /// 用户管理器
    /// </summary>
    protected IdentityUserManager UserManager { get; }
    /// <summary>
    /// 身份用户存储库
    /// </summary>
    protected IIdentityUserRepository IdentityUserRepository { get; }
    /// <summary>
    /// 身份选项
    /// </summary>
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="guidGenerator"></param>
    /// <param name="currentTenant"></param>
    /// <param name="userManager"></param>
    /// <param name="identityUserRepository"></param>
    /// <param name="identityOptions"></param>
    protected ExternalLoginProviderBase(
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IdentityUserManager userManager,
        IIdentityUserRepository identityUserRepository,
        IOptions<IdentityOptions> identityOptions)
    {
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
        UserManager = userManager;
        IdentityUserRepository = identityUserRepository;
        IdentityOptions = identityOptions;
    }

    /// <summary>
    /// 用于尝试通过此来源验证用户。
    /// </summary>
    /// <param name="userName">用户名或电子邮件地址</param>
    /// <param name="plainPassword">用户的明文密码</param>
    /// <returns>True，表示此用户已通过此来源的验证</returns>
    public abstract Task<bool> TryAuthenticateAsync(string userName, string plainPassword);

    /// <summary>
    /// 返回一个值，指示该源是否已启用。
    /// </summary>
    /// <returns></returns>
    public abstract Task<bool> IsEnabledAsync();

    /// <summary>
    /// 当用户通过此源进行身份验证但用户尚不存在时，将调用此方法。因此，源应该创建用户并填充属性。
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="providerName">此提供商的名称</param>
    /// <returns>新创建的用户</returns>
    public virtual async Task<IdentityUser> CreateUserAsync(string userName, string providerName)
    {
        await IdentityOptions.SetAsync();

        var externalUser = await GetUserInfoAsync(userName);

        return await CreateUserAsync(externalUser, userName, providerName);
    }

    /// <summary>
    /// 当用户通过此源进行身份验证但用户尚不存在时，将调用此方法。因此，源应该创建用户并填充属性。
    /// </summary>
    /// <param name="externalUser">外部用户信息</param>
    /// <param name="userName">用户名</param>
    /// <param name="providerName">此提供商的名称</param>
    /// <returns>新创建的用户</returns>
    protected virtual async Task<IdentityUser> CreateUserAsync(ExternalLoginUserInfo externalUser, string userName, string providerName)
    {
        NormalizeExternalLoginUserInfo(externalUser, userName);

        var user = new IdentityUser(GuidGenerator.Create(),userName,externalUser.Email,tenantId: CurrentTenant.Id);

        user.Name = externalUser.Name;
        user.Surname = externalUser.Surname;

        user.IsExternal = true;

        user.SetEmailConfirmed(externalUser.EmailConfirmed ?? false);
        user.SetPhoneNumber(externalUser.PhoneNumber, externalUser.PhoneNumberConfirmed ?? false);

        (await UserManager.CreateAsync(user)).CheckErrors();

        if (externalUser.TwoFactorEnabled != null)
        {
            (await UserManager.SetTwoFactorEnabledAsync(user, externalUser.TwoFactorEnabled.Value)).CheckErrors();
        }

        (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();
        (await UserManager.AddLoginAsync(user,new UserLoginInfo(providerName,externalUser.ProviderKey,providerName))).CheckErrors();

        return user;
    }

    /// <summary>
    /// 此方法在现有用户通过此源的身份验证后调用。它可用于通过源更新用户的某些属性。
    /// </summary>
    /// <param name="providerName">此提供商的名称</param>
    /// <param name="user">可更新的用户</param>
    public virtual async Task UpdateUserAsync(IdentityUser user, string providerName)
    {
        await IdentityOptions.SetAsync();

        var externalUser = await GetUserInfoAsync(user);

        await UpdateUserAsync(user, externalUser, providerName);
    }

    /// <summary>
    /// 更新外部用户
    /// </summary>
    /// <param name="user"></param>
    /// <param name="externalUser"></param>
    /// <param name="providerName"></param>
    /// <returns></returns>
    protected virtual async Task UpdateUserAsync(IdentityUser user, ExternalLoginUserInfo externalUser, string providerName)
    {
        NormalizeExternalLoginUserInfo(externalUser, user.UserName);

        if (!externalUser.Name.IsNullOrWhiteSpace())
        {
            user.Name = externalUser.Name;
        }

        if (!externalUser.Surname.IsNullOrWhiteSpace())
        {
            user.Surname = externalUser.Surname;
        }

        if (user.PhoneNumber != externalUser.PhoneNumber)
        {
            if (!externalUser.PhoneNumber.IsNullOrWhiteSpace())
            {
                await UserManager.SetPhoneNumberAsync(user, externalUser.PhoneNumber);
                user.SetPhoneNumberConfirmed(externalUser.PhoneNumberConfirmed == true);
            }
        }
        else
        {
            if (!user.PhoneNumber.IsNullOrWhiteSpace() &&
                user.PhoneNumberConfirmed == false &&
                externalUser.PhoneNumberConfirmed == true)
            {
                user.SetPhoneNumberConfirmed(true);
            }
        }

        if (!string.Equals(user.Email, externalUser.Email, StringComparison.OrdinalIgnoreCase))
        {
            (await UserManager.SetEmailAsync(user, externalUser.Email)).CheckErrors();
            user.SetEmailConfirmed(externalUser.EmailConfirmed ?? false);
        }

        if (externalUser.TwoFactorEnabled != null)
        {
            (await UserManager.SetTwoFactorEnabledAsync(user, externalUser.TwoFactorEnabled.Value)).CheckErrors();
        }

        await IdentityUserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins);

        var userLogin = user.Logins.FirstOrDefault(l => l.LoginProvider == providerName);
        if (userLogin != null)
        {
            if (userLogin.ProviderKey != externalUser.ProviderKey)
            {
                (await UserManager.RemoveLoginAsync(user, providerName, userLogin.ProviderKey)).CheckErrors();
                (await UserManager.AddLoginAsync(user, new UserLoginInfo(providerName, externalUser.ProviderKey, providerName))).CheckErrors();
            }
        }
        else
        {
            (await UserManager.AddLoginAsync(user, new UserLoginInfo(providerName, externalUser.ProviderKey, providerName))).CheckErrors();
        }

        user.IsExternal = true;

        (await UserManager.UpdateAsync(user)).CheckErrors();
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    protected abstract Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName);

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    protected virtual Task<ExternalLoginUserInfo> GetUserInfoAsync(IdentityUser user)
    {
        return GetUserInfoAsync(user.UserName);
    }

    /// <summary>
    /// 规范化外部登录用户信息
    /// </summary>
    /// <param name="externalUser"></param>
    /// <param name="userName"></param>
    private static void NormalizeExternalLoginUserInfo(ExternalLoginUserInfo externalUser,string userName)
    {
        if (externalUser.ProviderKey.IsNullOrWhiteSpace())
        {
            externalUser.ProviderKey = userName;
        }
    }
}
