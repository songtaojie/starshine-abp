using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Starshine.Abp.Identity;

/// <summary>
/// 用户声明主体工厂
/// </summary>
public class StarshineUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>,
    ITransientDependency
{
    /// <summary>
    /// 当前主要访问者
    /// </summary>
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
    /// <summary>
    /// Abp 声明主要工厂
    /// </summary>
    protected IAbpClaimsPrincipalFactory AbpClaimsPrincipalFactory { get; }

    /// <summary>
    /// 用户声明主体工厂
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="options"></param>
    /// <param name="currentPrincipalAccessor"></param>
    /// <param name="abpClaimsPrincipalFactory"></param>
    public StarshineUserClaimsPrincipalFactory(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory)
        : base(
            userManager,
            roleManager,
            options)
    {
        CurrentPrincipalAccessor = currentPrincipalAccessor;
        AbpClaimsPrincipalFactory = abpClaimsPrincipalFactory;
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [UnitOfWork]
    public async override Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = principal.Identities.First();

        if (user.TenantId.HasValue)
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString()));
        }

        if (!user.Name.IsNullOrWhiteSpace())
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.Name, user.Name));
        }

        if (!user.Surname.IsNullOrWhiteSpace())
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.SurName, user.Surname));
        }

        if (!user.PhoneNumber.IsNullOrWhiteSpace())
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.PhoneNumber, user.PhoneNumber));
        }

        identity.AddIfNotContains(
            new Claim(AbpClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed.ToString()));

        if (!user.Email.IsNullOrWhiteSpace())
        {
            identity.AddIfNotContains(new Claim(AbpClaimTypes.Email, user.Email));
        }

        identity.AddIfNotContains(new Claim(AbpClaimTypes.EmailVerified, user.EmailConfirmed.ToString()));

        using (CurrentPrincipalAccessor.Change(identity))
        {
            await AbpClaimsPrincipalFactory.CreateAsync(principal);
        }

        return principal;
    }
}
