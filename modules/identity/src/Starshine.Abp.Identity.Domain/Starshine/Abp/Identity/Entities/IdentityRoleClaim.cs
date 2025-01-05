using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Starshine.Abp.Identity;

/// <summary>
/// 表示授予角色内所有用户的声明。
/// </summary>
public class IdentityRoleClaim : IdentityClaim
{
    /// <summary>
    /// 获取或设置与此声明相关角色的主键。
    /// </summary>
    public virtual Guid RoleId { get; protected set; }
    /// <summary>
    /// 
    /// </summary>
    protected IdentityRoleClaim()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="roleId"></param>
    /// <param name="claim"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityRoleClaim(Guid id, Guid roleId, [NotNull] Claim claim,Guid? tenantId)
        : base(id, claim,tenantId)
    {
        RoleId = roleId;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="roleId"></param>
    /// <param name="claimType"></param>
    /// <param name="claimValue"></param>
    /// <param name="tenantId"></param>
    public IdentityRoleClaim(Guid id,Guid roleId,[NotNull] string claimType,string claimValue,Guid? tenantId)
        : base(id,claimType,claimValue,tenantId)
    {
        RoleId = roleId;
    }
}
