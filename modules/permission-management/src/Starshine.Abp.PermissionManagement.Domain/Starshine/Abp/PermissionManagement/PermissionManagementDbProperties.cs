using Volo.Abp.Data;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限管理数据库属性
/// </summary>
public static class PermissionManagementDbProperties
{
    /// <summary>
    /// 数据库前缀
    /// </summary>
    public static string DbTablePrefix { get; set; } = "Starshine";

    /// <summary>
    /// 数据库架构
    /// </summary>
    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public const string ConnectionStringName = "PermissionManagement";
}
