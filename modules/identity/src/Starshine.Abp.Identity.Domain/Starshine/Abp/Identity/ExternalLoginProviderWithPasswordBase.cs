using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 带有密码基础的外部登录提供程序
/// </summary>
public abstract class ExternalLoginProviderWithPasswordBase : ExternalLoginProviderBase, IExternalLoginProviderWithPassword
{
    /// <summary>
    /// 无需密码即可获取用户信息
    /// </summary>
    public bool CanObtainUserInfoWithoutPassword { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="guidGenerator"></param>
    /// <param name="currentTenant"></param>
    /// <param name="userManager"></param>
    /// <param name="identityUserRepository"></param>
    /// <param name="identityOptions"></param>
    /// <param name="canObtainUserInfoWithoutPassword"></param>
    public ExternalLoginProviderWithPasswordBase(
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IdentityUserManager userManager,
        IIdentityUserRepository identityUserRepository,
        IOptions<IdentityOptions> identityOptions,
        bool canObtainUserInfoWithoutPassword = false) :
        base(guidGenerator,
            currentTenant,
            userManager,
            identityUserRepository,
            identityOptions)
    {
        CanObtainUserInfoWithoutPassword = canObtainUserInfoWithoutPassword;
    }

    /// <summary>
    /// 当用户通过此源进行身份验证但用户尚不存在时，将调用此方法。因此，源应该创建用户并填充属性。
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="providerName">此提供商的名称</param>
    /// <param name="plainPassword">用户的明文密码</param>
    /// <returns>新创建的用户</returns>
    public async Task<IdentityUser> CreateUserAsync(string userName, string providerName, string plainPassword)
    {
        if (CanObtainUserInfoWithoutPassword)
        {
            return await CreateUserAsync(userName, providerName);
        }

        await IdentityOptions.SetAsync();

        var externalUser = await GetUserInfoAsync(userName, plainPassword);

        return await CreateUserAsync(externalUser, userName, providerName);
    }

    /// <summary>
    /// 此方法在现有用户通过此源的身份验证后调用。它可用于通过源更新用户的某些属性。
    /// </summary>
    /// <param name="providerName">此提供商的名称</param>
    /// <param name="user">可更新的用户</param>
    /// <param name="plainPassword">用户的明文密码</param>
    public async Task UpdateUserAsync(IdentityUser user, string providerName, string plainPassword)
    {
        if (CanObtainUserInfoWithoutPassword)
        {
            await UpdateUserAsync(user, providerName);
            return;
        }

        await IdentityOptions.SetAsync();

        var externalUser = await GetUserInfoAsync(user, plainPassword);

        await UpdateUserAsync(user, externalUser, providerName);
    }

    /// <summary>
    /// 获取用户
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName)
    {
        throw new NotImplementedException($"{nameof(GetUserInfoAsync)} is not implemented default. It should be overriden and implemented by the deriving class!");
    }

    /// <summary>
    /// 获取用户
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="plainPassword"></param>
    /// <returns></returns>
    protected abstract Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName, string plainPassword);

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="user"></param>
    /// <param name="plainPassword"></param>
    /// <returns></returns>
    protected virtual Task<ExternalLoginUserInfo> GetUserInfoAsync(IdentityUser user, string plainPassword)
    {
        return GetUserInfoAsync(user.UserName, plainPassword);
    }
}