using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.TenantManagement;

/// <summary>
/// 租户Eto
/// </summary>
[Serializable]
public class TenantEto : IHasEntityVersion
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 实体版本
    /// </summary>
    public int EntityVersion { get; set; }
}
