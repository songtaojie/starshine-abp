using System.Collections.Generic;

namespace Starshine.Abp.OpenIddict.WildcardDomains;

public class AbpOpenIddictWildcardDomainOptions
{
    public bool EnableWildcardDomainSupport { get; set; }

    public HashSet<string> WildcardDomainsFormat { get; }

    public AbpOpenIddictWildcardDomainOptions()
    {
        WildcardDomainsFormat = new HashSet<string>();
    }
}
