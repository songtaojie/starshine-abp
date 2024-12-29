namespace Starshine.Abp.Identity;

/// <summary>
/// 身份用户声明常量
/// </summary>
public static class IdentityUserClaimConsts
{
    /// <summary>
    /// 默认值: 256
    /// </summary>
    public static int MaxClaimTypeLength { get; set; } = 256;

    /// <summary>
    /// 默认值: 1024
    /// </summary>
    public static int MaxClaimValueLength { get; set; } = 1024;
}
