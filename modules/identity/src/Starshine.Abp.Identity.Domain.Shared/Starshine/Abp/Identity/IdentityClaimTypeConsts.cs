namespace Starshine.Abp.Identity;

/// <summary>
/// 身份声明类型常量
/// </summary>
public class IdentityClaimTypeConsts
{
    /// <summary>
    /// 最大名称长度
    /// 默认值: 256
    /// </summary>
    public static int MaxNameLength { get; set; } = 256;

    /// <summary>
    /// 最大正则表达式长度
    ///默认值: 512
    /// </summary>
    public static int MaxRegexLength { get; set; } = 512;

    /// <summary>
    /// 最大正则表达式描述长度
    /// 默认值: 128
    /// </summary>
    public static int MaxRegexDescriptionLength { get; set; } = 128;

    /// <summary>
    /// 最大描述长度
    /// 默认值: 256
    /// </summary>
    public static int MaxDescriptionLength { get; set; } = 256;
}
