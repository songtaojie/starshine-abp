namespace Starshine.Abp.Identity;

/// <summary>
/// 身份用户令牌常量
/// </summary>
public static class IdentityUserTokenConsts
{
    /// <summary>
    /// 默认值: 64
    /// </summary>
    public static int MaxLoginProviderLength { get; set; } = 64;

    /// <summary>
    /// 默认值: 128
    /// </summary>
    public static int MaxNameLength { get; set; } = 128;
}
