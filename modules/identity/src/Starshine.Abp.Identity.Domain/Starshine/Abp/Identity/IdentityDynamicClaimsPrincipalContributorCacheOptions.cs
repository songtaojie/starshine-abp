using System;

namespace Starshine.Abp.Identity;

public class IdentityDynamicClaimsPrincipalContributorCacheOptions
{
    public TimeSpan CacheAbsoluteExpiration { get; set; }

    public IdentityDynamicClaimsPrincipalContributorCacheOptions()
    {
        CacheAbsoluteExpiration = TimeSpan.FromHours(1);
    }
}
