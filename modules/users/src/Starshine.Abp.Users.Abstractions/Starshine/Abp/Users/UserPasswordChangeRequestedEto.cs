using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户密码变更事件对象
/// </summary>
[Serializable]
[EventName("Starshine.Abp.Users.UserPasswordChangeRequested")]
public class UserPasswordChangeRequestedEto : IMultiTenant
{
    /// <summary>
    /// 租户id
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public required string Password { get; set; }
}
