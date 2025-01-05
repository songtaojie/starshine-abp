using System;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份角色名称更改Eto
/// </summary>
[Serializable]
public class IdentityRoleNameChangedEto : IMultiTenant
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 旧名称
    /// </summary>
    public required string OldName { get; set; }
}
