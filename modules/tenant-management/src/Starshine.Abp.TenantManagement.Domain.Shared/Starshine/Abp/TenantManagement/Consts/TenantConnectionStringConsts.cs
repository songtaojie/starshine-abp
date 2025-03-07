namespace Starshine.Abp.TenantManagement;

/// <summary>
/// 租户连接字符串常量
/// </summary>
public static class TenantConnectionStringConsts
{
    /// <summary>
    /// Default value: 64
    /// </summary>
    public static int MaxNameLength { get; set; } = 64;

    /// <summary>
    /// Default value: 1024
    /// </summary>
    public static int MaxValueLength { get; set; } = 1024;
}
