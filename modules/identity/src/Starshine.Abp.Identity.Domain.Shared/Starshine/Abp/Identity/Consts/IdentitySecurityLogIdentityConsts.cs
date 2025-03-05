namespace Starshine.Abp.Identity;

/// <summary>
/// 身份安全日志身份常量
/// </summary>
public static class IdentitySecurityLogIdentityConsts
{
    /// <summary>
    /// 身份
    /// </summary>
    public static string Identity { get; set; } = "Identity";

    /// <summary>
    /// 身份外部
    /// </summary>
    public static string IdentityExternal { get; set; } = "IdentityExternal";

    /// <summary>
    /// 身份二因素
    /// </summary>
    public static string IdentityTwoFactor { get; set; } = "IdentityTwoFactor";
}
