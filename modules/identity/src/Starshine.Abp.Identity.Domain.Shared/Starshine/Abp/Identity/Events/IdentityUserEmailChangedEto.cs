using System;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份用户电子邮件更改Eto
/// </summary>
[Serializable]
public class IdentityUserEmailChangedEto : IMultiTenant
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
    /// 新的邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 旧的邮箱
    /// </summary>
    public string? OldEmail { get; set; }
}