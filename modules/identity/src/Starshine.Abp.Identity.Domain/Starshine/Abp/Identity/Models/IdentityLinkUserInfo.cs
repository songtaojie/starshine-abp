using System;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份链接用户信息
/// </summary>
/// <remarks>
/// </remarks>
/// <param name="userId"></param>
/// <param name="tenantId"></param>
public class IdentityLinkUserInfo(Guid userId, Guid? tenantId = null)
{
    /// <summary>
    /// 用户id
    /// </summary>
    public virtual Guid UserId { get; set; } = userId;
    /// <summary>
    /// 租户id
    /// </summary>
    public virtual Guid? TenantId { get; set; } = tenantId;
}
