namespace Starshine.Abp.IdentityServer.Consts;
/// <summary>
/// 设备流代码常量
/// </summary>
public class DeviceFlowCodesConsts
{
    /// <summary>
    /// 设备代码最大长度
    /// 默认值：200
    /// </summary>
    public static int DeviceCodeMaxLength { get; set; } = 200;

    /// <summary>
    /// 用户代码最大长度
    /// 默认值：200
    /// </summary>
    public static int UserCodeMaxLength { get; set; } = 200;

    /// <summary>
    /// 科目ID最大长度
    /// 默认值：200
    /// </summary>
    public static int SubjectIdMaxLength { get; set; } = 200;

    /// <summary>
    /// 会话ID最大长度
    /// 默认值：100
    /// </summary>
    public static int SessionIdMaxLength { get; set; } = 100;

    /// <summary>
    /// 描述最大长度
    /// 默认值：200
    /// </summary>
    public static int DescriptionMaxLength { get; set; } = 200;

    /// <summary>
    /// ClientId最大长度
    /// 默认值：200
    /// </summary>
    public static int ClientIdMaxLength { get; set; } = 200;

    /// <summary>
    /// 数据最大长度
    /// 默认值：200
    /// </summary>
    public static int DataMaxLength { get; set; } = 50000;
}
