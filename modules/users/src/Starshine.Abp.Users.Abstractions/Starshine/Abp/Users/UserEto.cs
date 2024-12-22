using Volo.Abp.Data;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户事件传输对象模型
/// </summary>
[EventName("Starshine.Abp.Users.User")]
public class UserEto : IUserData, IMultiTenant
{
    /// <summary>
    /// 用户主键id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 用户名字
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 用户姓
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// 是否激活
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 邮箱是否确认
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 手机号是否确认
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// 额外属性
    /// </summary>
    public ExtraPropertyDictionary ExtraProperties { get; set; } = [];
}
