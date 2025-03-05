namespace Starshine.Abp.Identity;

/// <summary>
/// StarshineAbp 身份选项
/// </summary>
public class StarshineIdentityOptions
{
    /// <summary>
    /// 外部登录提供者词典
    /// </summary>
    public ExternalLoginProviderDictionary ExternalLoginProviders { get; }

    /// <summary>
    /// StarshineAbp 身份选项
    /// </summary>
    public StarshineIdentityOptions()
    {
        ExternalLoginProviders = [];
    }
}
