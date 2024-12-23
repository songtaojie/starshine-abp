namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 授权常量
/// </summary>
public static class PermissionGrantConsts
{
    /// <summary>
    /// 服务商名字长度
    /// 默认值: 64
    /// </summary>
    public static int MaxProviderNameLength { get; set; } = 64;

    /// <summary>
    /// 服务商key长度
    ///  默认值: 64
    /// </summary>
    public static int MaxProviderKeyLength { get; set; } = 64;
}
