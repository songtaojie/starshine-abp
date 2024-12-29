namespace Starshine.Abp.Identity;

/// <summary>
/// 身份角色声明常量
/// </summary>
public static class IdentityRoleClaimConsts
{
    /// <summary>
    /// 最大声明类型长度
    /// </summary>
    public static int MaxClaimTypeLength { get; set; } = IdentityUserClaimConsts.MaxClaimTypeLength;

    /// <summary>
    /// 最大声明值长度
    /// </summary>
    public static int MaxClaimValueLength { get; set; } = IdentityUserClaimConsts.MaxClaimValueLength;
}
