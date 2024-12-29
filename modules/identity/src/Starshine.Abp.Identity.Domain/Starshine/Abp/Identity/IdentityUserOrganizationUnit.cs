using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 表示用户对 OU 的成员资格。
/// </summary>
public class IdentityUserOrganizationUnit : CreationAuditedEntity, IMultiTenant
{
    /// <summary>
    /// 此实体的 TenantId。
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// 用户的 ID。
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// 相关 <see cref="OrganizationUnit"/> 的 ID。
    /// </summary>
    public virtual Guid OrganizationUnitId { get; protected set; }
    /// <summary>
    /// 
    /// </summary>
    protected IdentityUserOrganizationUnit()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="organizationUnitId"></param>
    /// <param name="tenantId"></param>
    public IdentityUserOrganizationUnit(Guid userId, Guid organizationUnitId, Guid? tenantId = null)
    {
        UserId = userId;
        OrganizationUnitId = organizationUnitId;
        TenantId = tenantId;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [UserId, OrganizationUnitId];
    }
}
