namespace Starshine.Abp.IdentityServer.Consts;
/// <summary>
/// 客户端常量
/// </summary>
public class ClientConsts
{
    /// <summary>
    /// 客户端Id最大长度
    /// </summary>
    public static int ClientIdMaxLength { get; set; } = 200;

    /// <summary>
    /// 客户端协议类型最大长度
    /// </summary>
    public static int ProtocolTypeMaxLength { get; set; } = 200;

    /// <summary>
    /// 客户端名称最大长度
    /// </summary>
    public static int ClientNameMaxLength { get; set; } = 200;

    /// <summary>
    /// 客户端Uri最大长度
    /// </summary>
    public static int ClientUriMaxLength { get; set; } = 2000;

    /// <summary>
    /// 客户端LogoUri最大长度
    /// </summary>
    public static int LogoUriMaxLength { get; set; } = 2000;

    /// <summary>
    /// 描述最大长度
    /// </summary>
    public static int DescriptionMaxLength { get; set; } = 1000;

    /// <summary>
    /// 前置通道注销 Uri 最大长度
    /// </summary>
    public static int FrontChannelLogoutUriMaxLength { get; set; } = 2000;

    /// <summary>
    /// 后置通道注销 Uri 最大长度
    /// </summary>
    public static int BackChannelLogoutUriMaxLength { get; set; } = 2000;

    /// <summary>
    /// 客户端跳转Uri最大长度
    /// </summary>
    public static int ClientClaimsPrefixMaxLength { get; set; } = 200;

    /// <summary>
    /// 成对主题盐最大长度
    /// </summary>
    public static int PairWiseSubjectSaltMaxLength { get; set; } = 200;

    /// <summary>
    /// 用户代码类型最大长度
    /// </summary>
    public static int UserCodeTypeMaxLength { get; set; } = 100;

    /// <summary>
    /// 允许身份令牌签名算法
    /// </summary>
    public static int AllowedIdentityTokenSigningAlgorithms { get; set; } = 100;
}
