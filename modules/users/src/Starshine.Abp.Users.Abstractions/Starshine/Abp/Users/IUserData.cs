using Volo.Abp.Data;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户数据
/// </summary>
public interface IUserData : IHasExtraProperties
{
    /// <summary>
    /// 用户id
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// 租户id
    /// </summary>
    Guid? TenantId { get; }

    /// <summary>
    /// 用户名
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// 用户名字
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// 用户姓
    /// </summary>
    string? Surname { get; }

    /// <summary>
    /// 是否激活
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// 邮箱
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// 邮箱是否确认
    /// </summary>
    bool EmailConfirmed { get; }

    /// <summary>
    /// 手机号码
    /// </summary>
    string? PhoneNumber { get; }

    /// <summary>
    /// 手机号是否确认
    /// </summary>
    bool PhoneNumberConfirmed { get; }
}
