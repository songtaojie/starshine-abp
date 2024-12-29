using System.Collections.ObjectModel;
using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Starshine.Abp.Users;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份用户
/// </summary>
public class IdentityUser : FullAuditedAggregateRoot<Guid>, IUser, IHasEntityVersion
{
    /// <summary>
    /// 租户id
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// 获取或设置此用户的用户名。
    /// </summary>
    public virtual string UserName { get; protected internal set; } = null!;

    /// <summary>
    ///获取或设置此用户的规范化用户名。
    /// </summary>
    [DisableAuditing]
    public virtual string NormalizedUserName { get; protected internal set; } = null!;

    /// <summary>
    /// 获取或设置用户的名称。
    /// </summary>
    [CanBeNull]
    public virtual string Name { get; set; } = string.Empty;

    /// <summary>
    /// 获取或设置用户的姓氏。
    /// </summary>
    [CanBeNull]
    public virtual string? Surname { get; set; }

    /// <summary>
    /// 获取或设置此用户的电子邮件地址。
    /// </summary>
    public virtual string Email { get; protected internal set; } = null!;

    /// <summary>
    /// 获取或设置此用户的规范化电子邮件地址。
    /// </summary>
    [DisableAuditing]
    public virtual string? NormalizedEmail { get; protected internal set; }

    /// <summary>
    ///获取或设置一个标志，指示用户是否已确认其电子邮件地址。
    /// </summary>
    /// <value>如果电子邮件地址已确认则为 True，否则为 false.</value>
    public virtual bool EmailConfirmed { get; protected internal set; }

    /// <summary>
    /// 获取或设置此用户的密码的加盐和散列表示形式。
    /// </summary>
    [DisableAuditing]
    public virtual string PasswordHash { get; protected internal set; } = null!;

    /// <summary>
    /// 当用户凭证发生改变（密码更改、登录删除）时必须更改的随机值
    /// </summary>
    [DisableAuditing]
    public virtual string SecurityStamp { get; protected internal set; } = null!;

    /// <summary>
    /// 是否为外部
    /// </summary>
    public virtual bool IsExternal { get; set; }

    /// <summary>
    /// 获取或设置用户的电话号码。
    /// </summary>
    [CanBeNull]
    public virtual string? PhoneNumber { get; protected internal set; }

    /// <summary>
    /// 获取或设置一个标志，指示用户是否已确认其电话地址。
    /// </summary>
    /// <value>如果电话号码已确认则为 True，否则为 false.</value>
    public virtual bool PhoneNumberConfirmed { get; protected internal set; }

    /// <summary>
    ///获取或设置指示用户是否处于活动状态的标志。
    /// </summary>
    public virtual bool IsActive { get; protected internal set; }

    /// <summary>
    ///获取或设置一个标志，指示是否为该用户启用双因素身份验证。
    /// </summary>
    /// <value>如果启用了 2fa，则为 True，否则为 false.</value>
    public virtual bool TwoFactorEnabled { get; protected internal set; }

    /// <summary>
    /// 获取或设置任何用户锁定的结束日期和时间（UTC）。
    /// </summary>
    /// <remarks>
    ///过去的值意味着用户没有被锁定。
    /// </remarks>
    public virtual DateTimeOffset? LockoutEnd { get; protected internal set; }

    /// <summary>
    ///获取或设置一个标志，指示用户是否可以被锁定。
    /// </summary>
    /// <value>如果用户可能被锁定则为 True，否则为 false.</value>
    public virtual bool LockoutEnabled { get; protected internal set; }

    /// <summary>
    /// 获取或设置当前用户登录失败的次数。
    /// </summary>
    public virtual int AccessFailedCount { get; protected internal set; }

    /// <summary>
    /// 下次登录时应更改密码。
    /// </summary>
    public virtual bool ShouldChangePasswordOnNextLogin { get; protected internal set; }

    /// <summary>
    ///每当实体发生变化时，版本值就会增加。
    /// </summary>
    public virtual int EntityVersion { get; protected set; }

    /// <summary>
    ///获取或设置用户上次更改密码的时间。
    /// </summary>
    public virtual DateTimeOffset? LastPasswordChangeTime { get; protected set; }

    /// <summary>
    /// 此用户所属角色的导航属性。
    /// </summary>
    public virtual ICollection<IdentityUserRole> Roles { get; protected set; } = null!;

    /// <summary>
    ///此用户拥有的声明的导航属性。
    /// </summary>
    public virtual ICollection<IdentityUserClaim> Claims { get; protected set; } = null!;

    /// <summary>
    ///此用户登录帐户的导航属性。
    /// </summary>
    public virtual ICollection<IdentityUserLogin> Logins { get; protected set; } = null!;

    /// <summary>
    /// 此用户令牌的导航属性。
    /// </summary>
    public virtual ICollection<IdentityUserToken> Tokens { get; protected set; } = null!;

    /// <summary>
    ///此组织单位的导航属性。
    /// </summary>
    public virtual ICollection<IdentityUserOrganizationUnit> OrganizationUnits { get; protected set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    protected IdentityUser()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="tenantId"></param>
    public IdentityUser( Guid id,[NotNull] string userName,[NotNull] string email, Guid? tenantId = null): base(id)
    {
        Check.NotNull(userName, nameof(userName));
        Check.NotNull(email, nameof(email));

        TenantId = tenantId;
        UserName = userName;
        NormalizedUserName = userName.ToUpperInvariant();
        Email = email;
        NormalizedEmail = email.ToUpperInvariant();
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        SecurityStamp = Guid.NewGuid().ToString();
        IsActive = true;

        Roles = new Collection<IdentityUserRole>();
        Claims = new Collection<IdentityUserClaim>();
        Logins = new Collection<IdentityUserLogin>();
        Tokens = new Collection<IdentityUserToken>();
        OrganizationUnits = new Collection<IdentityUserOrganizationUnit>();
    }

    /// <summary>
    /// 添加角色
    /// </summary>
    /// <param name="roleId"></param>
    public virtual void AddRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (IsInRole(roleId))
        {
            return;
        }

        Roles.Add(new IdentityUserRole(Id, roleId, TenantId));
    }

    /// <summary>
    /// 移除角色
    /// </summary>
    /// <param name="roleId"></param>
    public virtual void RemoveRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (!IsInRole(roleId))
        {
            return;
        }

        Roles.RemoveAll(r => r.RoleId == roleId);
    }

    /// <summary>
    /// 是否在角色中
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public virtual bool IsInRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        return Roles.Any(r => r.RoleId == roleId);
    }

    /// <summary>
    /// 添加声明
    /// </summary>
    /// <param name="guidGenerator"></param>
    /// <param name="claim"></param>
    public virtual void AddClaim([NotNull] IGuidGenerator guidGenerator, [NotNull] Claim claim)
    {
        Check.NotNull(guidGenerator, nameof(guidGenerator));
        Check.NotNull(claim, nameof(claim));

        Claims.Add(new IdentityUserClaim(guidGenerator.Create(), Id, claim, TenantId));
    }

    /// <summary>
    /// 添加声明
    /// </summary>
    /// <param name="guidGenerator"></param>
    /// <param name="claims"></param>
    public virtual void AddClaims([NotNull] IGuidGenerator guidGenerator, [NotNull] IEnumerable<Claim> claims)
    {
        Check.NotNull(guidGenerator, nameof(guidGenerator));
        Check.NotNull(claims, nameof(claims));

        foreach (var claim in claims)
        {
            AddClaim(guidGenerator, claim);
        }
    }

    /// <summary>
    /// 获取声明
    /// </summary>
    /// <param name="claim"></param>
    /// <returns></returns>
    public virtual IdentityUserClaim? FindClaim([NotNull] Claim claim)
    {
        Check.NotNull(claim, nameof(claim));

        return Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
    }

    /// <summary>
    /// 替换声明
    /// </summary>
    /// <param name="claim"></param>
    /// <param name="newClaim"></param>
    public virtual void ReplaceClaim([NotNull] Claim claim, [NotNull] Claim newClaim)
    {
        Check.NotNull(claim, nameof(claim));
        Check.NotNull(newClaim, nameof(newClaim));

        var userClaims = Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
        foreach (var userClaim in userClaims)
        {
            userClaim.SetClaim(newClaim);
        }
    }

    /// <summary>
    /// 移除声明
    /// </summary>
    /// <param name="claims"></param>
    public virtual void RemoveClaims([NotNull] IEnumerable<Claim> claims)
    {
        Check.NotNull(claims, nameof(claims));

        foreach (var claim in claims)
        {
            RemoveClaim(claim);
        }
    }

    /// <summary>
    /// 移除声明
    /// </summary>
    /// <param name="claim"></param>
    public virtual void RemoveClaim([NotNull] Claim claim)
    {
        Check.NotNull(claim, nameof(claim));

        Claims.RemoveAll(c => c.ClaimValue == claim.Value && c.ClaimType == claim.Type);
    }

    /// <summary>
    /// 添加登录信息
    /// </summary>
    /// <param name="login"></param>
    public virtual void AddLogin([NotNull] UserLoginInfo login)
    {
        Check.NotNull(login, nameof(login));

        Logins.Add(new IdentityUserLogin(Id, login, TenantId));
    }

    /// <summary>
    /// 移除登录信息
    /// </summary>
    /// <param name="loginProvider"></param>
    /// <param name="providerKey"></param>
    public virtual void RemoveLogin([NotNull] string loginProvider, [NotNull] string providerKey)
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));

        Logins.RemoveAll(userLogin =>
            userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);
    }

    /// <summary>
    /// 查找令牌
    /// </summary>
    /// <param name="loginProvider"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual IdentityUserToken? FindToken(string loginProvider, string name)
    {
        return Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
    }

    /// <summary>
    /// 设置令牌
    /// </summary>
    /// <param name="loginProvider"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public virtual void SetToken(string loginProvider, string name, string value)
    {
        var token = FindToken(loginProvider, name);
        if (token == null)
        {
            Tokens.Add(new IdentityUserToken(Id, loginProvider, name, value, TenantId));
        }
        else
        {
            token.Value = value;
        }
    }

    /// <summary>
    /// 移除令牌
    /// </summary>
    /// <param name="loginProvider"></param>
    /// <param name="name"></param>
    public virtual void RemoveToken(string loginProvider, string name)
    {
        Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
    }

    /// <summary>
    /// 添加组织单位
    /// </summary>
    /// <param name="organizationUnitId"></param>
    public virtual void AddOrganizationUnit(Guid organizationUnitId)
    {
        if (IsInOrganizationUnit(organizationUnitId))
        {
            return;
        }

        OrganizationUnits.Add(
            new IdentityUserOrganizationUnit(
                Id,
                organizationUnitId,
                TenantId
            )
        );
    }

    /// <summary>
    /// 移除组织单元
    /// </summary>
    /// <param name="organizationUnitId"></param>
    public virtual void RemoveOrganizationUnit(Guid organizationUnitId)
    {
        if (!IsInOrganizationUnit(organizationUnitId))
        {
            return;
        }

        OrganizationUnits.RemoveAll(ou => ou.OrganizationUnitId == organizationUnitId);
    }

    /// <summary>
    /// 是否在组织单位
    /// </summary>
    /// <param name="organizationUnitId"></param>
    /// <returns></returns>
    public virtual bool IsInOrganizationUnit(Guid organizationUnitId)
    {
        return OrganizationUnits.Any(ou => ou.OrganizationUnitId == organizationUnitId);
    }

    /// <summary>
    /// 使用 <see cref="UserManager{TUser}.ConfirmEmailAsync(TUser, string)"/> 进行常规电子邮件确认。
    /// 使用此项可跳过确认过程并直接设置 <see cref="EmailConfirmed"/>。
    /// </summary>
    public virtual void SetEmailConfirmed(bool confirmed)
    {
        EmailConfirmed = confirmed;
    }

    /// <summary>
    /// 设置电话号码确认
    /// </summary>
    /// <param name="confirmed"></param>
    public virtual void SetPhoneNumberConfirmed(bool confirmed)
    {
        PhoneNumberConfirmed = confirmed;
    }

    /// <summary>
    ///通常使用 <see cref="UserManager{TUser}.ChangePhoneNumberAsync"/> 在应用程序代码中更改电话号码。此方法是直接为其设置确认信息。
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="confirmed"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetPhoneNumber(string? phoneNumber, bool confirmed)
    {
        PhoneNumber = phoneNumber;
        PhoneNumberConfirmed = !phoneNumber.IsNullOrWhiteSpace() && confirmed;
    }

    /// <summary>
    /// 激活状态
    /// </summary>
    /// <param name="isActive"></param>
    public virtual void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }

    /// <summary>
    /// 设置下次登录时应更改密码
    /// </summary>
    /// <param name="shouldChangePasswordOnNextLogin"></param>
    public virtual void SetShouldChangePasswordOnNextLogin(bool shouldChangePasswordOnNextLogin)
    {
        ShouldChangePasswordOnNextLogin = shouldChangePasswordOnNextLogin;
    }

    /// <summary>
    /// 设置上次密码更改时间
    /// </summary>
    /// <param name="lastPasswordChangeTime"></param>
    public virtual void SetLastPasswordChangeTime(DateTimeOffset? lastPasswordChangeTime)
    {
        LastPasswordChangeTime = lastPasswordChangeTime;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{base.ToString()}, UserName = {UserName}";
    }
}
