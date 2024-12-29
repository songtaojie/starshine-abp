using System;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 组织单位Eto
/// </summary>
[Serializable]
public class OrganizationUnitEto : IMultiTenant, IHasEntityVersion
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
    /// 组织单位编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public int EntityVersion { get; set; }
}
