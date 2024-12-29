using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 表示用户和角色之间的联系。
/// </summary>
public class IdentityUserRole : Entity, IMultiTenant
{
    /// <summary>
    /// 租户id
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// 获取或设置与角色相关联的用户的主键。
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// 获取或设置与用户关联的角色的主键。
    /// </summary>
    public virtual Guid RoleId { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    protected IdentityUserRole()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityUserRole(Guid userId, Guid roleId, Guid? tenantId)
    {
        UserId = userId;
        RoleId = roleId;
        TenantId = tenantId;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [UserId, RoleId];
    }
}
