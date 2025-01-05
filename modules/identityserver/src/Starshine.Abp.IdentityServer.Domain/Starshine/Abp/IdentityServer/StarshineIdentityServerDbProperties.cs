using Volo.Abp.Data;

namespace Starshine.Abp.IdentityServer;

public static class StarshineIdentityServerDbProperties
{
    public static string DbTablePrefix { get; set; } = "IdentityServer";

    public static string? DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

    public const string ConnectionStringName = "StarshineIdentityServer";
}
