using Starshine.Abp.Users;
using Volo.Abp.Users;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份用户常量
/// </summary>
public static class IdentityUserConsts
{
    /// <summary>
    /// 最大用户名长度
    /// </summary>
    public static int MaxUserNameLength { get; set; } = StarshineAbpUserConsts.MaxUserNameLength;

    /// <summary>
    /// 最大名称长度
    /// </summary>
    public static int MaxNameLength { get; set; } = StarshineAbpUserConsts.MaxNameLength;

    /// <summary>
    /// 最大姓氏长度
    /// </summary>
    public static int MaxSurnameLength { get; set; } = StarshineAbpUserConsts.MaxSurnameLength;

    /// <summary>
    /// 最大规范化用户名长度
    /// </summary>
    public static int MaxNormalizedUserNameLength { get; set; } = MaxUserNameLength;

    /// <summary>
    /// 最大电子邮件长度
    /// </summary>
    public static int MaxEmailLength { get; set; } = StarshineAbpUserConsts.MaxEmailLength;

    /// <summary>
    /// 最大规范化电子邮件长度
    /// </summary>
    public static int MaxNormalizedEmailLength { get; set; } = MaxEmailLength;

    /// <summary>
    /// 最大电话号码长度
    /// </summary>
    public static int MaxPhoneNumberLength { get; set; } = StarshineAbpUserConsts.MaxPhoneNumberLength;

    /// <summary>
    /// 最大密码长度
    /// 默认值: 128
    /// </summary>
    public static int MaxPasswordLength { get; set; } = 128;

    /// <summary>
    /// 最大密码哈希长度
    /// 默认值: 256
    /// </summary>
    public static int MaxPasswordHashLength { get; set; } = 256;

    /// <summary>
    /// 最大安全标记长度
    /// 默认值: 256
    /// </summary>
    public static int MaxSecurityStampLength { get; set; } = 256;

    /// <summary>
    /// 最大登录提供商长度
    /// 默认值: 16
    /// </summary>
    public static int MaxLoginProviderLength { get; set; } = 16;
}
