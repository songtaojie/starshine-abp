using System;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份用户用户名更改Eto
/// </summary>
[Serializable]
public class IdentityUserUserNameChangedEto : IMultiTenant
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 旧的用户名
    /// </summary>
    public string? OldUserName { get; set; }
}
