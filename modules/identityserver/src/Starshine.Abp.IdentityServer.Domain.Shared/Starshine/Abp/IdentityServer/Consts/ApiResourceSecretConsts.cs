namespace Starshine.Abp.IdentityServer.Consts;
/// <summary>
/// Api资源密钥常量
/// </summary>
public class ApiResourceSecretConsts
{
    /// <summary>
    /// Default value: 250
    /// </summary>
    public static int TypeMaxLength { get; set; } = 250;

    /// <summary>
    /// Default value: 4000
    /// </summary>
    public static int ValueMaxLength { get; set; } = 4000;

    /// <summary>
    /// Default value: 1000
    /// </summary>
    public static int DescriptionMaxLength { get; set; } = 1000;
}
