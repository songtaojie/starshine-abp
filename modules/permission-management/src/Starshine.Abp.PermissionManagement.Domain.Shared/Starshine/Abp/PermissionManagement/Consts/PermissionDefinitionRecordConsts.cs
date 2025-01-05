namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限定义记录常量
/// </summary>
public class PermissionDefinitionRecordConsts
{
    /// <summary>
    /// 名字长度
    /// 默认值: 128
    /// </summary>
    public static int MaxNameLength { get; set; } = 128;

    /// <summary>
    /// DisplayName长度
    /// 默认值256
    /// </summary>
    public static int MaxDisplayNameLength { get; set; } = 256;

    /// <summary>
    /// 服务商长度
    /// 默认值128
    /// </summary>
    public static int MaxProvidersLength { get; set; } = 128;

    /// <summary>
    /// 状态检查长度
    /// 默认值256
    /// </summary>
    public static int MaxStateCheckersLength { get; set; } = 256;
}