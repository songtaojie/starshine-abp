using System.Security.Claims;
using IdentityModel;
using Starshine.IdentityServer.AspNetIdentity;
using Starshine.IdentityServer.Models;
using Starshine.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Starshine.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using IdentityUser = Starshine.Abp.Identity.IdentityUser;
using Starshine.Abp.IdentityServer.Localization;
using Starshine.Abp.Identity.Managers;
using Starshine.Abp.IdentityServer.Consts;

namespace Starshine.Abp.IdentityServer.AspNetIdentity;
/// <summary>
/// ��Դ������������֤��
/// </summary>
public class StarshineResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    /// <summary>
    /// ��¼������
    /// </summary>
    protected SignInManager<IdentityUser> SignInManager { get; }
    /// <summary>
    /// �û�������
    /// </summary>
    protected IdentityUserManager UserManager { get; }
    /// <summary>
    /// ��ݰ�ȫ��־������
    /// </summary>
    protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
    /// <summary>
    /// ��־
    /// </summary>
    protected ILogger<ResourceOwnerPasswordValidator<IdentityUser>> Logger { get; }

    /// <summary>
    /// ���ػ�
    /// </summary>
    protected IStringLocalizer<StarshineIdentityServerResource> Localizer { get; }

    /// <summary>
    /// ����Χ����
    /// </summary>
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    /// <summary>
    /// ���ѡ��
    /// </summary>
    protected StarshineIdentityOptions StarshineIdentityOptions { get; }

    /// <summary>
    /// ���ѡ��
    /// </summary>
    protected IOptions<IdentityOptions> IdentityOptions { get; }

    /// <summary>
    /// �����ṩ��
    /// </summary>
    protected ISettingProvider SettingProvider { get; }

    /// <summary>
    ///     ���캯��
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="identitySecurityLogManager"></param>
    /// <param name="logger"></param>
    /// <param name="localizer"></param>
    /// <param name="starshineIdentityOptions"></param>
    /// <param name="serviceScopeFactory"></param>
    /// <param name="identityOptions"></param>
    /// <param name="settingProvider"></param>
    public StarshineResourceOwnerPasswordValidator(
        IdentityUserManager userManager,
        SignInManager<IdentityUser> signInManager,
        IdentitySecurityLogManager identitySecurityLogManager,
        ILogger<ResourceOwnerPasswordValidator<IdentityUser>> logger,
        IStringLocalizer<StarshineIdentityServerResource> localizer,
        IOptions<StarshineIdentityOptions> starshineIdentityOptions,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<IdentityOptions> identityOptions,
        ISettingProvider settingProvider)
    {
        UserManager = userManager;
        SignInManager = signInManager;
        IdentitySecurityLogManager = identitySecurityLogManager;
        Logger = logger;
        Localizer = localizer;
        ServiceScopeFactory = serviceScopeFactory;
        StarshineIdentityOptions = starshineIdentityOptions.Value;
        IdentityOptions = identityOptions;
        SettingProvider = settingProvider;
    }

    /// <summary>
    /// https://github.com/IdentityServer/Starshine.IdentityServer/blob/master/src/AspNetIdentity/src/ResourceOwnerPasswordValidator.cs#L53
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            await ReplaceEmailToUsernameOfInputIfNeeds(context);

            IdentityUser? user = null;

            if (StarshineIdentityOptions.ExternalLoginProviders.Any())
            {
                foreach (var externalLoginProviderInfo in StarshineIdentityOptions.ExternalLoginProviders.Values)
                {
                    var externalLoginProvider = (IExternalLoginProvider)scope.ServiceProvider
                        .GetRequiredService(externalLoginProviderInfo.Type);

                    if (await externalLoginProvider.TryAuthenticateAsync(context.UserName, context.Password))
                    {
                        user = await UserManager.FindByNameAsync(context.UserName);
                        if (user == null)
                        {
                            user = await externalLoginProvider.CreateUserAsync(context.UserName, externalLoginProviderInfo.Name);
                        }
                        else
                        {
                            await externalLoginProvider.UpdateUserAsync(user, externalLoginProviderInfo.Name);
                        }

                        await SetSuccessResultAsync(context, user);
                        return;
                    }
                }
            }

            user = await UserManager.FindByNameAsync(context.UserName);
            string errorDescription;
            if (user != null)
            {
                await IdentityOptions.SetAsync();
                var result = await SignInManager.CheckPasswordSignInAsync(user, context.Password, true);
                if (result.Succeeded)
                {
                    if (await IsTfaEnabledAsync(user))
                    {
                        await HandleTwoFactorLoginAsync(context, user);
                    }
                    else
                    {
                        await SetSuccessResultAsync(context, user);
                    }
                    return;
                }

                if (result.IsLockedOut)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                    errorDescription = Localizer["UserLockedOut"];
                }
                else if (result.IsNotAllowed)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);

                    if (user.ShouldChangePasswordOnNextLogin)
                    {
                        await HandleShouldChangePasswordOnNextLoginAsync(context, user, context.Password);
                        return;
                    }

                    if (await UserManager.ShouldPeriodicallyChangePasswordAsync(user))
                    {
                        await HandlePeriodicallyChangePasswordAsync(context, user, context.Password);
                        return;
                    }

                    errorDescription = Localizer["LoginIsNotAllowed"];
                }
                else
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                    errorDescription = Localizer["InvalidUserNameOrPassword"];
                }

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                {
                    Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                    Action = result.ToIdentitySecurityLogAction(),
                    UserName = context.UserName,
                    ClientId = await FindClientIdAsync(context)
                });
            }
            else
            {
                Logger.LogInformation("No user found matching username: {username}", context.UserName);
                errorDescription = Localizer["InvalidUsername"];

                await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
                {
                    Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                    Action = IdentityServerSecurityLogActionConsts.LoginInvalidUserName,
                    UserName = context.UserName,
                    ClientId = await FindClientIdAsync(context)
                });
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorDescription);
        }
    }
    /// <summary>
    /// ����˫���ص�¼
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    protected virtual async Task HandleTwoFactorLoginAsync(ResourceOwnerPasswordValidationContext context, IdentityUser user)
    {
        var recoveryCode = context.Request?.Raw?["RecoveryCode"];
        if (!recoveryCode.IsNullOrWhiteSpace())
        {
            var result = await UserManager.RedeemTwoFactorRecoveryCodeAsync(user, recoveryCode);
            if (result.Succeeded)
            {
                await SetSuccessResultAsync(context, user);
                return;
            }

            Logger.LogInformation("Authentication failed for username: {username}, reason: InvalidRecoveryCode", context.UserName);
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, Localizer["InvalidRecoveryCode"]);
        }

        var twoFactorProvider = context.Request?.Raw?["TwoFactorProvider"];
        var twoFactorCode = context.Request?.Raw?["TwoFactorCode"];
        if (!twoFactorProvider.IsNullOrWhiteSpace() && !twoFactorCode.IsNullOrWhiteSpace())
        {
            var providers = await UserManager.GetValidTwoFactorProvidersAsync(user);
            if (providers.Contains(twoFactorProvider) && await UserManager.VerifyTwoFactorTokenAsync(user, twoFactorProvider, twoFactorCode))
            {
                await SetSuccessResultAsync(context, user);
                return;
            }

            await UserManager.AccessFailedAsync(user);

            Logger.LogInformation("Authentication failed for username: {username}, reason: InvalidAuthenticatorCode", context.UserName);
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, Localizer["InvalidAuthenticatorCode"]);
        }
        else
        {
            Logger.LogInformation("Authentication failed for username: {username}, reason: RequiresTwoFactor", context.UserName);
            var twoFactorToken = await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, nameof(SignInResult.RequiresTwoFactor));
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, nameof(SignInResult.RequiresTwoFactor),
                new Dictionary<string, object>()
                {
                        {"userId", user.Id},
                        {"twoFactorToken", twoFactorToken}
                });

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                Action = IdentityServerSecurityLogActionConsts.LoginRequiresTwoFactor,
                UserName = context.UserName,
                ClientId = await FindClientIdAsync(context)
            });
        }
    }

    /// <summary>
    /// �����´ε�¼ʱӦ��������
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <param name="currentPassword"></param>
    /// <returns></returns>
    protected virtual async Task HandleShouldChangePasswordOnNextLoginAsync(ResourceOwnerPasswordValidationContext context, IdentityUser user, string currentPassword)
    {
        await HandlerChangePasswordAsync(context, user, currentPassword, ChangePasswordType.ShouldChangePasswordOnNextLogin);
    }

    /// <summary>
    /// �����ڸ�������
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <param name="currentPassword"></param>
    /// <returns></returns>
    protected virtual async Task HandlePeriodicallyChangePasswordAsync(ResourceOwnerPasswordValidationContext context, IdentityUser user, string currentPassword)
    {
        await HandlerChangePasswordAsync(context, user, currentPassword, ChangePasswordType.PeriodicallyChangePassword);
    }

    /// <summary>
    /// �����û��������
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <param name="currentPassword"></param>
    /// <param name="changePasswordType"></param>
    /// <returns></returns>
    protected virtual async Task HandlerChangePasswordAsync(ResourceOwnerPasswordValidationContext context, IdentityUser user, string currentPassword, ChangePasswordType changePasswordType)
    {
        var changePasswordToken = context.Request?.Raw?["ChangePasswordToken"];
        var newPassword = context.Request?.Raw?["NewPassword"];
        if (!changePasswordToken.IsNullOrWhiteSpace() && !currentPassword.IsNullOrWhiteSpace() && !newPassword.IsNullOrWhiteSpace())
        {
            if (await UserManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, changePasswordType.ToString(), changePasswordToken))
            {
                var changePasswordResult = await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (changePasswordResult.Succeeded)
                {
                    await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
                    {
                        Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                        Action = IdentitySecurityLogActionConsts.ChangePassword,
                        UserName = context.UserName,
                        ClientId = await FindClientIdAsync(context)
                    });

                    if (changePasswordType == ChangePasswordType.ShouldChangePasswordOnNextLogin)
                    {
                        user.SetShouldChangePasswordOnNextLogin(false);
                    }

                    await UserManager.UpdateAsync(user);
                    await SetSuccessResultAsync(context, user);
                }
                else
                {
                    Logger.LogInformation("ChangePassword failed for username: {username}, reason: {changePasswordResult}", context.UserName, changePasswordResult);
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, changePasswordResult.Errors.Select(x => x.Description).JoinAsString(", "));
                }
            }
            else
            {
                Logger.LogInformation("Authentication failed for username: {username}, reason: InvalidAuthenticatorCode", context.UserName);
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, Localizer["InvalidAuthenticatorCode"]);
            }
        }
        else
        {
            Logger.LogInformation($"Authentication failed for username: {{{context.UserName}}}, reason: {{{changePasswordType.ToString()}}}");
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, nameof(user.ShouldChangePasswordOnNextLogin),
                new Dictionary<string, object>()
                {
                        {"userId", user.Id},
                        {"changePasswordToken", await UserManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, changePasswordType.ToString())}
                });

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                Action = IdentityServerSecurityLogActionConsts.LoginNotAllowed,
                UserName = context.UserName,
                ClientId = await FindClientIdAsync(context)
            });
        }
    }

    /// <summary>
    /// ���óɹ����
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    protected virtual async Task SetSuccessResultAsync(ResourceOwnerPasswordValidationContext context, IdentityUser user)
    {
        var sub = await UserManager.GetUserIdAsync(user);

        Logger.LogInformation("Credentials validated for username: {username}", context.UserName);

        var additionalClaims = new List<Claim>();

        await AddCustomClaimsAsync(additionalClaims, user, context);

        context.Result = new GrantValidationResult(
            sub,
            OidcConstants.AuthenticationMethods.Password,
            additionalClaims.ToArray()
        );

        await IdentitySecurityLogManager.SaveAsync(
            new IdentitySecurityLogContext
            {
                Identity = IdentityServerSecurityLogIdentityConsts.IdentityServer,
                Action = IdentityServerSecurityLogActionConsts.LoginSucceeded,
                UserName = context.UserName,
                ClientId = await FindClientIdAsync(context)
            }
        );
    }

    /// <summary>
    /// �����Ҫ���뽫�����ʼ��滻Ϊ�û���
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(ResourceOwnerPasswordValidationContext context)
    {
        if (!ValidationHelper.IsValidEmailAddress(context.UserName))
        {
            return;
        }

        var userByUsername = await UserManager.FindByNameAsync(context.UserName);
        if (userByUsername != null)
        {
            return;
        }

        var userByEmail = await UserManager.FindByEmailAsync(context.UserName);
        if (userByEmail == null)
        {
            return;
        }

        context.UserName = userByEmail.UserName;
    }

    /// <summary>
    /// ���ҿͻ���ID
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual Task<string?> FindClientIdAsync(ResourceOwnerPasswordValidationContext context)
    {
        return Task.FromResult(context.Request?.Client?.ClientId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    protected virtual async Task<bool> IsTfaEnabledAsync(IdentityUser user)
        => UserManager.SupportsUserTwoFactor &&
           await UserManager.GetTwoFactorEnabledAsync(user) &&
           (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;

    /// <summary>
    /// ����Զ�������
    /// </summary>
    /// <param name="customClaims"></param>
    /// <param name="user"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual Task AddCustomClaimsAsync(List<Claim> customClaims, IdentityUser user, ResourceOwnerPasswordValidationContext context)
    {
        if (user.TenantId.HasValue)
        {
            customClaims.Add(new Claim( AbpClaimTypes.TenantId, user.TenantId.Value.ToString()));
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// �����޸�����
    /// </summary>
    public enum ChangePasswordType
    {
        /// <summary>
        /// �´ε�¼ʱӦ��������
        /// </summary>
        ShouldChangePasswordOnNextLogin,
        /// <summary>
        /// ���ڸ�������
        /// </summary>
        PeriodicallyChangePassword
    }
}
