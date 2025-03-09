using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using IdentityUser = Starshine.Abp.Identity.IdentityUser;

namespace Starshine.Abp.IdentityServer.AspNetIdentity;
/// <summary>
/// �ӿ��������幤��
/// </summary>
/// <typeparam name="TUser"></typeparam>
public class StarshineUserClaimsFactory<TUser> : IUserClaimsPrincipalFactory<TUser>
    where TUser : class
{
    /// <summary>
    ///
    /// </summary>
    protected IObjectAccessor<IUserClaimsPrincipalFactory<TUser>> Inner { get; }
    /// <summary>
    /// �û�������
    /// </summary>
    protected UserManager<TUser> UserManager { get; }
    /// <summary>
    /// ��ǰ�û�������
    /// </summary>
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
    /// <summary>
    /// ABP�������幤��
    /// </summary>
    protected IAbpClaimsPrincipalFactory AbpClaimsPrincipalFactory { get; }
    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="inner"></param>
    /// <param name="userManager"></param>
    /// <param name="currentPrincipalAccessor"></param>
    /// <param name="abpClaimsPrincipalFactory"></param>
    public StarshineUserClaimsFactory(
        IObjectAccessor<IUserClaimsPrincipalFactory<TUser>> inner,
        UserManager<TUser> userManager,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory)
    {
        Inner = inner;
        UserManager = userManager;
        CurrentPrincipalAccessor = currentPrincipalAccessor;
        AbpClaimsPrincipalFactory = abpClaimsPrincipalFactory;
    }

    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public virtual async Task<ClaimsPrincipal> CreateAsync(TUser user)
    {
        var principal = await Inner.Value!.CreateAsync(user);
        var identity = principal.Identities.First();

        if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Subject))
        {
            var sub = await UserManager.GetUserIdAsync(user);
            identity.AddIfNotContains(new Claim(JwtClaimTypes.Subject, sub));
        }

        var username = await UserManager.GetUserNameAsync(user);
        var usernameClaim = identity.FindFirst(claim =>
            claim.Type == UserManager.Options.ClaimsIdentity.UserNameClaimType && claim.Value == username);
        if (usernameClaim != null)
        {
            identity.RemoveClaim(usernameClaim);
            if (!string.IsNullOrWhiteSpace(username))
            {
                identity.AddIfNotContains(new Claim(JwtClaimTypes.PreferredUserName, username));
                //https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1627
                //https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/05e02b5e0383be40e45c667c12f6667d38e33fcc/src/System.IdentityModel.Tokens.Jwt/ClaimTypeMapping.cs#L52
                identity.AddIfNotContains(new Claim(JwtRegisteredClaimNames.UniqueName, username));
            }
        }

        if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Name) && !string.IsNullOrWhiteSpace(username))
        {
            identity.AddIfNotContains(new Claim(JwtClaimTypes.Name, username));
        }

        if (UserManager.SupportsUserEmail)
        {
            var email = await UserManager.GetEmailAsync(user);
            if (!string.IsNullOrWhiteSpace(email))
            {
                identity.AddIfNotContains(new Claim(JwtClaimTypes.Email, email));
                identity.AddIfNotContains(new Claim(JwtClaimTypes.EmailVerified,
                    await UserManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean));
            }
        }

        if (UserManager.SupportsUserPhoneNumber)
        {
            var phoneNumber = await UserManager.GetPhoneNumberAsync(user);
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                identity.AddIfNotContains(new Claim(JwtClaimTypes.PhoneNumber, phoneNumber));
                identity.AddIfNotContains(new Claim(JwtClaimTypes.PhoneNumberVerified,
                    await UserManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false",
                    ClaimValueTypes.Boolean));
            }
        }

        if (user is IdentityUser identityUser)
        {
            if (!identityUser.Name.IsNullOrEmpty())
            {
                identity.AddIfNotContains(new Claim(JwtClaimTypes.GivenName, identityUser.Name));
            }

            if (!identityUser.Surname.IsNullOrEmpty())
            {
                identity.AddIfNotContains(new Claim(JwtClaimTypes.FamilyName, identityUser.Surname));
            }
        }

        using (CurrentPrincipalAccessor.Change(identity))
        {
            await AbpClaimsPrincipalFactory.CreateAsync(principal);
        }

        return principal;
    }
}
