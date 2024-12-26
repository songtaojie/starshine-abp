using Microsoft.AspNetCore.Identity;
using static Starshine.Abp.Identity.AspNetCore.AbpSecurityStampValidatorCallback;

namespace Starshine.Abp.Identity.AspNetCore;

public static class SecurityStampValidatorOptionsExtensions
{
    public static SecurityStampValidatorOptions UpdatePrincipal(this SecurityStampValidatorOptions options, AbpRefreshingPrincipalOptions abpRefreshingPrincipalOptions)
    {
        var previousOnRefreshingPrincipal = options.OnRefreshingPrincipal;
        options.OnRefreshingPrincipal = async context =>
        {
            await SecurityStampValidatorCallback.UpdatePrincipal(context, abpRefreshingPrincipalOptions);
            if (previousOnRefreshingPrincipal != null)
            {
                await previousOnRefreshingPrincipal.Invoke(context);
            }
        };
        return options;
    }
}
