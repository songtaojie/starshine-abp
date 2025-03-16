using Volo.Abp.Data;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// IdentityServerDb属性
/// </summary>
public static class IdentityServerDbProperties
{
    /// <summary>
    /// 数据库表前缀
    /// </summary>
    public static string DbTablePrefix { get; set; } = "Starshine";

    /// <summary>
    /// 数据库架构
    /// </summary>
    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    /// <summary>
    /// 连接字符串名称
    /// </summary>
    public const string ConnectionStringName = "StarshineIdentityServer";
}
