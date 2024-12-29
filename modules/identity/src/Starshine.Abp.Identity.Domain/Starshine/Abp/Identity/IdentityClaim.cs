using System;
using System.Security.Claims;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 认证声明
/// </summary>
public abstract class IdentityClaim : Entity<Guid>, IMultiTenant
{
    /// <summary>
    /// 租户id
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// 获取或设置此声明的声明类型。
    /// </summary>
    public virtual string ClaimType { get; protected set; } = string.Empty;

    /// <summary>
    ///获取或设置此声明的声明值。
    /// </summary>
    public virtual string ClaimValue { get; protected set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    protected IdentityClaim()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="claim"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityClaim(Guid id, [NotNull] Claim claim, Guid? tenantId)
        : this(id, claim.Type, claim.Value, tenantId)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="claimType"></param>
    /// <param name="claimValue"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityClaim(Guid id, [NotNull] string claimType, string claimValue, Guid? tenantId)
    {
        Check.NotNull(claimType, nameof(claimType));
        Id = id;
        ClaimType = claimType;
        ClaimValue = claimValue;
        TenantId = tenantId;
    }

    /// <summary>
    /// 从该实体创建一个 Claim 实例。
    /// </summary>
    /// <returns></returns>
    public virtual Claim ToClaim()
    {
        return new Claim(ClaimType, ClaimValue);
    }

    /// <summary>
    /// 设置声明
    /// </summary>
    /// <param name="claim"></param>
    public virtual void SetClaim([NotNull] Claim claim)
    {
        Check.NotNull(claim, nameof(claim));

        ClaimType = claim.Type;
        ClaimValue = claim.Value;
    }
}
