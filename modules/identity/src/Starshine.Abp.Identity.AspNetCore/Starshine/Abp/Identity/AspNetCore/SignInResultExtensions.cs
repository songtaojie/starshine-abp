using Microsoft.AspNetCore.Identity;

namespace Starshine.Abp.Identity.AspNetCore;

/// <summary>
/// 登录结果扩展
/// </summary>
public static class SignInResultExtensions
{
    /// <summary>
    /// 身份安全日志操作
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static string ToIdentitySecurityLogAction(this SignInResult result)
    {
        if (result.Succeeded)
        {
            return IdentitySecurityLogActionConsts.LoginSucceeded;
        }

        if (result.IsLockedOut)
        {
            return IdentitySecurityLogActionConsts.LoginLockedout;
        }

        if (result.RequiresTwoFactor)
        {
            return IdentitySecurityLogActionConsts.LoginRequiresTwoFactor;
        }

        if (result.IsNotAllowed)
        {
            return IdentitySecurityLogActionConsts.LoginNotAllowed;
        }

        if (!result.Succeeded)
        {
            return IdentitySecurityLogActionConsts.LoginFailed;
        }

        return IdentitySecurityLogActionConsts.LoginFailed;
    }
}
