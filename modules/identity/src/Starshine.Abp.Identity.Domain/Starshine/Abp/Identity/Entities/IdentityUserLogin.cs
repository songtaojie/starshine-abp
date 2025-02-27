using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
///代表用户的登录名及其相关提供程序。
/// </summary>
public class IdentityUserLogin : Entity, IMultiTenant
{
    /// <summary>
    /// 租户id
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    ///获取或设置与此登录关联的用户的主键。
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// 获取或设置登录的登录提供商（例如 facebook、google）
    /// </summary>
    public virtual string LoginProvider { get; protected set; }

    /// <summary>
    /// 获取或设置此登录的唯一提供程序标识符。
    /// </summary>
    public virtual string ProviderKey { get; protected set; } = string.Empty;

    /// <summary>
    /// 获取或设置此登录在 UI 中使用的友好名称。
    /// </summary>
    public virtual string? ProviderDisplayName { get; protected set; }
   
    /// <summary>
    /// 构造函数。
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="loginProvider"></param>
    /// <param name="providerKey"></param>
    /// <param name="providerDisplayName"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityUserLogin(Guid userId, string loginProvider, string providerKey, string? providerDisplayName, Guid? tenantId)
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));

        UserId = userId;
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
        ProviderDisplayName = providerDisplayName;
        TenantId = tenantId;
    }
    /// <summary>
    /// 构造函数。
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="login"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityUserLogin(Guid userId, UserLoginInfo login, Guid? tenantId)
        : this(userId, login.LoginProvider, login.ProviderKey, login.ProviderDisplayName, tenantId)
    {
    }

    /// <summary>
    /// 转换为 <see cref="UserLoginInfo"/>
    /// </summary>
    /// <returns></returns>
    public virtual UserLoginInfo ToUserLoginInfo()
    {
        return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
    }
    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [UserId, LoginProvider];
    }
}
