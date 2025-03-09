namespace Starshine.Abp.IdentityServer;
/// <summary>
/// 允许 Cors 来源缓存项目
/// </summary>
public class AllowedCorsOriginsCacheItem
{
    /// <summary>
    /// 所有来源
    /// </summary>
    public const string AllOrigins = "AllOrigins";

    /// <summary>
    /// 允许的来源
    /// </summary>
    public string[] AllowedOrigins { get; set; } = [];
}
