namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限组定义记录常量
/// </summary>
public class PermissionGroupDefinitionRecordConsts
{
    /// <summary>
    /// 组名称长度
    /// 默认值: 128
    /// </summary>
    public static int MaxNameLength { get; set; } = 128;

    /// <summary>
    /// 组显示名称长度
    /// 默认值256
    /// </summary>
    public static int MaxDisplayNameLength { get; set; } = 256;
}