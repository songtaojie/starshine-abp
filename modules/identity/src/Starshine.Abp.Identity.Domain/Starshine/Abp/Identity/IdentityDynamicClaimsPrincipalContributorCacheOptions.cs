using System;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份动态声明主要贡献者缓存选项
/// </summary>
public class IdentityDynamicClaimsPrincipalContributorCacheOptions
{
    /// <summary>
    /// 
    /// </summary>
    public TimeSpan CacheAbsoluteExpiration { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public IdentityDynamicClaimsPrincipalContributorCacheOptions()
    {
        CacheAbsoluteExpiration = TimeSpan.FromHours(1);
    }
}
