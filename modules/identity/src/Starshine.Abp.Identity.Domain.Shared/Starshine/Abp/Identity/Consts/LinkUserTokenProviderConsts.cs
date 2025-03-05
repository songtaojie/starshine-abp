namespace Starshine.Abp.Identity;

/// <summary>
/// 链接用户令牌提供者 Consts
/// </summary>
public static class LinkUserTokenProviderConsts
{
    /// <summary>
    /// 链接用户令牌提供商名称
    /// </summary>
    public static string LinkUserTokenProviderName { get; set; } = "StarshineLinkUser";

    /// <summary>
    /// 链接用户令牌目的
    /// </summary>
    public static string LinkUserTokenPurpose { get; set; } = "StarshineLinkUser";

    /// <summary>
    /// 链接用户登录令牌目的
    /// </summary>
    public static string LinkUserLoginTokenPurpose { get; set; } = "StarshineLinkUserLogin";
}
