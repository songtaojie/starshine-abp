namespace Starshine.Abp.IdentityServer.Consts;

/// <summary>
/// Api资源常量
/// </summary>
public class ApiResourceConsts
{
    /// <summary>
    /// 名字最大长度
    /// 默认值：200
    /// </summary>
    public static int NameMaxLength { get; set; } = 200;

    /// <summary>
    /// 显示名字最大长度
    /// 默认值：200
    /// </summary>
    public static int DisplayNameMaxLength { get; set; } = 200;

    /// <summary>
    /// 描述最大长度
    /// 默认值：1000
    /// </summary>
    public static int DescriptionMaxLength { get; set; } = 1000;

    /// <summary>
    /// 允许访问令牌签名算法最大长度
    /// 默认值：100
    /// </summary>
    public static int AllowedAccessTokenSigningAlgorithmsMaxLength { get; set; } = 100;
}
