namespace Starshine.Abp.Users;

/// <summary>
/// 用户常量
/// </summary>
public class StarshineAbpUserConsts
{
    /// <summary>
    /// 用户名长度，默认值: 256
    /// </summary>
    public static int MaxUserNameLength { get; set; } = 64;

    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxNameLength { get; set; } = 64;

    /// <summary>
    /// 姓长度，默认值: 64
    /// </summary>
    public static int MaxSurnameLength { get; set; } = 64;

    /// <summary>
    ///邮箱长度，默认值: 128
    /// </summary>
    public static int MaxEmailLength { get; set; } = 128;

    /// <summary>
    /// 手机号码长度，默认值: 20
    /// </summary>
    public static int MaxPhoneNumberLength { get; set; } = 20;

    /// <summary>
    /// 用户时间传输对象模型
    /// </summary>
    public static string UserEventName { get; set; } = "Starshine.Abp.Users.User";
}
