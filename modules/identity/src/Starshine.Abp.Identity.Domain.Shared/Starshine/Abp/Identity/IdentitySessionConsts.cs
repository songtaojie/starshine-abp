namespace Starshine.Abp.Identity;

/// <summary>
/// 身份会话常量
/// </summary>
public class IdentitySessionConsts
{
    /// <summary>
    /// 最大会话ID长度
    /// </summary>
    public static int MaxSessionIdLength { get; set; } = 128;

    /// <summary>
    /// 最大设备长度
    /// </summary>
    public static int MaxDeviceLength { get; set; } = 64;

    /// <summary>
    /// 最大设备信息长度
    /// </summary>
    public static int MaxDeviceInfoLength { get; set; } = 64;

    /// <summary>
    /// 最大客户端ID长度
    /// </summary>
    public static int MaxClientIdLength { get; set; } = 64;

    /// <summary>
    /// 最大IP地址长度
    /// </summary>
    public static int MaxIpAddressesLength { get; set; } = 256;
}
