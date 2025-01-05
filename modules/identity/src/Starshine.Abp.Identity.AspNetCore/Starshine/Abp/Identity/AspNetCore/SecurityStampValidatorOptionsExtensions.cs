using Microsoft.AspNetCore.Identity;
using static Starshine.Abp.Identity.AspNetCore.StarshineSecurityStampValidatorCallback;

namespace Starshine.Abp.Identity.AspNetCore;

/// <summary>
/// 安全标记验证器选项扩展
/// </summary>
public static class SecurityStampValidatorOptionsExtensions
{
    /// <summary>
    /// 更新Principal
    /// </summary>
    /// <param name="options"></param>
    /// <param name="abpRefreshingPrincipalOptions"></param>
    /// <returns></returns>
    public static SecurityStampValidatorOptions UpdatePrincipal(this SecurityStampValidatorOptions options, StarshineRefreshingPrincipalOptions abpRefreshingPrincipalOptions)
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
