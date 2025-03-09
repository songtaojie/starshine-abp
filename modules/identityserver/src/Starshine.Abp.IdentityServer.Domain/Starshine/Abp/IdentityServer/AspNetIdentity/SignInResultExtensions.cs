using Microsoft.AspNetCore.Identity;
using Starshine.Abp.IdentityServer.Consts;

namespace Starshine.Abp.IdentityServer.AspNetIdentity;
/// <summary>
/// 登录结果扩展
/// </summary>
public static class SignInResultExtensions
{
    /// <summary>
    /// 转换为安全日志的登录操作
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
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
