﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Starshine.Abp.Identity.Settings;
using Volo.Abp.Settings;
using Volo.Abp.Timing;

namespace Starshine.Abp.Identity.AspNetCore;

public class AbpSignInManager : SignInManager<IdentityUser>
{
    protected StarshineIdentityOptions AbpOptions { get; }

    protected ISettingProvider SettingProvider { get; }

    private readonly IdentityUserManager _identityUserManager;

    public AbpSignInManager(
        IdentityUserManager userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<IdentityUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<IdentityUser> confirmation,
        IOptions<StarshineIdentityOptions> options,
        ISettingProvider settingProvider) : base(
        userManager,
        contextAccessor,
        claimsFactory,
        optionsAccessor,
        logger,
        schemes,
        confirmation)
    {
        SettingProvider = settingProvider;
        AbpOptions = options.Value;
        _identityUserManager = userManager;
    }

    
    /// <summary>
    /// 密码登录
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="password">用户密码</param>
    /// <param name="isPersistent">是否持久化</param>
    /// <param name="lockoutOnFailure">失败时是否锁定</param>
    /// <returns></returns>
    public async override Task<SignInResult> PasswordSignInAsync(string userName,string password, bool isPersistent,bool lockoutOnFailure)
    {
        foreach (var externalLoginProviderInfo in AbpOptions.ExternalLoginProviders.Values)
        {
            var externalLoginProvider = (IExternalLoginProvider)Context.RequestServices
                .GetRequiredService(externalLoginProviderInfo.Type);

            if (await externalLoginProvider.TryAuthenticateAsync(userName, password))
            {
                var user = await UserManager.FindByNameAsync(userName);
                if (user == null)
                {
                    if (externalLoginProvider is IExternalLoginProviderWithPassword externalLoginProviderWithPassword)
                    {
                        user = await externalLoginProviderWithPassword.CreateUserAsync(userName, externalLoginProviderInfo.Name, password);
                    }
                    else
                    {
                        user = await externalLoginProvider.CreateUserAsync(userName, externalLoginProviderInfo.Name);
                    }
                }
                else
                {
                    if (externalLoginProvider is IExternalLoginProviderWithPassword externalLoginProviderWithPassword)
                    {
                        await externalLoginProviderWithPassword.UpdateUserAsync(user, externalLoginProviderInfo.Name, password);
                    }
                    else
                    {
                        await externalLoginProvider.UpdateUserAsync(user, externalLoginProviderInfo.Name);
                    }
                }

                return await SignInOrTwoFactorAsync(user, isPersistent);
            }
        }

        return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
    }

    protected async override Task<SignInResult> PreSignInCheck(IdentityUser user)
    {
        if (!user.IsActive)
        {
            Logger.LogWarning($"The user is not active therefore cannot login! (username: \"{user.UserName}\", id:\"{user.Id}\")");
            return SignInResult.NotAllowed;
        }

        if (user.ShouldChangePasswordOnNextLogin)
        {
            Logger.LogWarning($"The user should change password! (username: \"{user.UserName}\", id:\"{user.Id}\")");
            return SignInResult.NotAllowed;
        }

        if (await _identityUserManager.ShouldPeriodicallyChangePasswordAsync(user))
        {
            return SignInResult.NotAllowed;
        }

        return await base.PreSignInCheck(user);
    }

    /// <summary>
    /// This is to call the protection method SignInOrTwoFactorAsync
    /// </summary>
    /// <param name="user"></param>
    /// <param name="isPersistent"></param>
    /// <param name="loginProvider"></param>
    /// <param name="bypassTwoFactor"></param>
    /// <returns></returns>
    public virtual async Task<SignInResult> CallSignInOrTwoFactorAsync(IdentityUser user, bool isPersistent, string loginProvider = null, bool bypassTwoFactor = false)
    {
        return await base.SignInOrTwoFactorAsync(user, isPersistent, loginProvider, bypassTwoFactor);
    }
}
