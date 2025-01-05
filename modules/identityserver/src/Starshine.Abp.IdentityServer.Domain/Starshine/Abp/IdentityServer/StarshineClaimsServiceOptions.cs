using System.Collections.Generic;

namespace Starshine.Abp.IdentityServer;

public class StarshineClaimsServiceOptions
{
    public List<string> RequestedClaims { get; }

    public StarshineClaimsServiceOptions()
    {
        RequestedClaims = new List<string>();
    }
}
