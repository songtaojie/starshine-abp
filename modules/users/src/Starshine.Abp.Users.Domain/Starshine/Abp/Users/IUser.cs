using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Users;

/// <summary>
/// 
/// </summary>
public interface IUser : IAggregateRoot<Guid>, IMultiTenant, IHasExtraProperties
{
    /// <summary>
    /// 用户名
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [CanBeNull]
    string? Email { get; }

    /// <summary>
    /// 用户名字
    /// </summary>
    [CanBeNull]
    string? Name { get; }

    /// <summary>
    /// 用户姓
    /// </summary>
    [CanBeNull]
    string? Surname { get; }

    /// <summary>
    /// 是否激活
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// 邮箱是否确认
    /// </summary>
    bool EmailConfirmed { get; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [CanBeNull]
    string? PhoneNumber { get; }

    /// <summary>
    /// 手机号是否确认
    /// </summary>
    bool PhoneNumberConfirmed { get; }
}
