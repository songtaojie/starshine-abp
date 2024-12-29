using System;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份角色Eto
/// </summary>
[Serializable]
public class IdentityRoleEto : IMultiTenant, IHasEntityVersion
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
    /// 名字
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// 是否静态
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    /// 是否共用
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 实体版本
    /// </summary>
    public int EntityVersion { get; set; }
}
