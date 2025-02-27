using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 代表用户的身份验证令牌。
/// </summary>
public class IdentityUserToken : Entity, IMultiTenant
{
    /// <summary>
    /// 租户id
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// 获取或设置令牌所属用户的主键。
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// 获取或设置此令牌所属的 LoginProvider。
    /// </summary>
    public virtual string LoginProvider { get; protected set; }

    /// <summary>
    /// 获取或设置令牌的名称。
    /// </summary>
    public virtual string Name { get; protected set; } 

    /// <summary>
    /// 获取或设置令牌值。
    /// </summary>
    public virtual string? Value { get; set; }

    /// <summary>
    /// 构造函数。
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="loginProvider"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityUserToken(Guid userId,string loginProvider,string name,string? value,Guid? tenantId)
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(name, nameof(name));
        UserId = userId;
        LoginProvider = loginProvider;
        Name = name;
        Value = value;
        TenantId = tenantId;
    }

    /// <summary>
    /// 获取此实体的主键。
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [UserId, LoginProvider, Name];
    }
}
