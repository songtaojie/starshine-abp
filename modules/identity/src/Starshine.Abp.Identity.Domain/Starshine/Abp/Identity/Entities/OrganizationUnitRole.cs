using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
///表示角色和组织单位之间的联系。
/// </summary>
public class OrganizationUnitRole : CreationAuditedEntity, IMultiTenant
{
    /// <summary>
    /// 此实体的 TenantId。
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// 角色id
    /// </summary>
    public virtual Guid RoleId { get; protected set; }

    /// <summary>
    /// <see cref="OrganizationUnit"/> 的 ID。
    /// </summary>
    public virtual Guid OrganizationUnitId { get; protected set; }

    /// <summary>
    /// 初始化 <see cref="OrganizationUnitRole"/> 类的新实例。
    /// </summary>
    protected OrganizationUnitRole()
    {

    }

    /// <summary>
    /// 初始化 <see cref="OrganizationUnitRole"/> 类的新实例。
    /// </summary>
    /// <param name="tenantId">租户id</param>
    /// <param name="roleId">角色id</param>
    /// <param name="organizationUnitId">Id of the <see cref="OrganizationUnit"/>.</param>
    public OrganizationUnitRole(Guid roleId, Guid organizationUnitId, Guid? tenantId = null)
    {
        RoleId = roleId;
        OrganizationUnitId = organizationUnitId;
        TenantId = tenantId;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [OrganizationUnitId, RoleId];
    }
}
