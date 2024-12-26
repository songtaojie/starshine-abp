using System.Collections.Generic;

namespace Starshine.Abp.IdentityServer;

public class AbpClaimsServiceOptions
{
    public List<string> RequestedClaims { get; }

    public AbpClaimsServiceOptions()
    {
        RequestedClaims = new List<string>();
    }
}
