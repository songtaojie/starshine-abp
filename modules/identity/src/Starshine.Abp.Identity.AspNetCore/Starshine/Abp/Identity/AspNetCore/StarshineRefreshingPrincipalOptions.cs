using System.Collections.Generic;
using Volo.Abp.Security.Claims;

namespace Starshine.Abp.Identity.AspNetCore;
/// <summary>
/// 刷新主体选项
/// </summary>
public class StarshineRefreshingPrincipalOptions
{
    /// <summary>
    /// 当前主体保留声明类型
    /// </summary>
    public List<string> CurrentPrincipalKeepClaimTypes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public StarshineRefreshingPrincipalOptions()
    {
        CurrentPrincipalKeepClaimTypes =
        [
            AbpClaimTypes.SessionId
        ];
    }
}
