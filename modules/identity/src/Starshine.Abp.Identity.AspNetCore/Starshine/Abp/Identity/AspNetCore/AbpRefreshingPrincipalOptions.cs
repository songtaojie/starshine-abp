using System.Collections.Generic;
using Volo.Abp.Security.Claims;

namespace Starshine.Abp.Identity.AspNetCore;

public class AbpRefreshingPrincipalOptions
{
    public List<string> CurrentPrincipalKeepClaimTypes { get; set; }

    public AbpRefreshingPrincipalOptions()
    {
        CurrentPrincipalKeepClaimTypes = new List<string>
        {
            AbpClaimTypes.SessionId
        };
    }
}
