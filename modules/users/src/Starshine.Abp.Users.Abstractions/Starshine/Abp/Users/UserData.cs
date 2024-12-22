using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Starshine.Abp.Users;
/// <summary>
/// 用户数据
/// </summary>
public class UserData : IUserData
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
    /// 用户昵称
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 用户名称
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
    public ExtraPropertyDictionary ExtraProperties { get; }

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public UserData()
    {
        ExtraProperties = [];
    }

    /// <summary>
    /// 用户构造函数
    /// </summary>
    /// <param name="userData"></param>
    public UserData(IUserData userData)
    {
        Id = userData.Id;
        UserName = userData.UserName;
        Email = userData.Email;
        Name = userData.Name;
        Surname = userData.Surname;
        IsActive = userData.IsActive;
        EmailConfirmed = userData.EmailConfirmed;
        PhoneNumber = userData.PhoneNumber;
        PhoneNumberConfirmed = userData.PhoneNumberConfirmed;
        TenantId = userData.TenantId;
        ExtraProperties = userData.ExtraProperties;
    }

    /// <summary>
    /// 用户数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="emailConfirmed"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="phoneNumberConfirmed"></param>
    /// <param name="tenantId"></param>
    /// <param name="isActive"></param>
    /// <param name="extraProperties"></param>
    public UserData(
        Guid id,
        [NotNull] string userName,
        [CanBeNull] string? email = null,
        [CanBeNull] string? name = null,
        [CanBeNull] string? surname = null,
        bool emailConfirmed = false,
        [CanBeNull] string? phoneNumber = null,
        bool phoneNumberConfirmed = false,
        Guid? tenantId = null,
        bool isActive = true,
        ExtraPropertyDictionary? extraProperties = null)
    {
        Id = id;
        UserName = userName;
        Email = email;
        Name = name;
        Surname = surname;
        IsActive = isActive;
        EmailConfirmed = emailConfirmed;
        PhoneNumber = phoneNumber;
        PhoneNumberConfirmed = phoneNumberConfirmed;
        TenantId = tenantId;
        ExtraProperties = extraProperties ?? [];
    }
}
