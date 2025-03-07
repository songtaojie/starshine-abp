using Microsoft.AspNetCore.Identity;
using Starshine.Abp.IdentityServer.Consts;

namespace Starshine.Abp.IdentityServer.AspNetIdentity;

public static class SignInResultExtensions
{
    public static string ToIdentitySecurityLogAction(this SignInResult result)
    {
        if (result.Succeeded)
        {
            return IdentityServerSecurityLogActionConsts.LoginSucceeded;
        }

        if (result.IsLockedOut)
        {
            return IdentityServerSecurityLogActionConsts.LoginLockedout;
        }

        if (result.RequiresTwoFactor)
        {
            return IdentityServerSecurityLogActionConsts.LoginRequiresTwoFactor;
        }

        if (result.IsNotAllowed)
        {
            return IdentityServerSecurityLogActionConsts.LoginNotAllowed;
        }

        if (!result.Succeeded)
        {
            return IdentityServerSecurityLogActionConsts.LoginFailed;
        }

        return IdentityServerSecurityLogActionConsts.LoginFailed;
    }
}
