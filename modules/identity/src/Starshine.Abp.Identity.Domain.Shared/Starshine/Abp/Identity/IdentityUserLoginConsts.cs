namespace Starshine.Abp.Identity;

/// <summary>
/// 身份用户登录常量
/// </summary>
public static class IdentityUserLoginConsts
{
    /// <summary>
    /// 默认值: 64
    /// </summary>
    public static int MaxLoginProviderLength { get; set; } = 64;

    /// <summary>
    /// 默认值: 196
    /// </summary>
    public static int MaxProviderKeyLength { get; set; } = 196;

    /// <summary>
    /// 默认值: 128
    /// </summary>
    public static int MaxProviderDisplayNameLength { get; set; } = 128;
}
