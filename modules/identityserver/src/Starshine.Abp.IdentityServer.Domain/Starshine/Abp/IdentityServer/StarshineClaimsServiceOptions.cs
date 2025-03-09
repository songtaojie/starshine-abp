using System.Collections.Generic;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// 声明服务配置
/// </summary>
public class StarshineClaimsServiceOptions
{
    /// <summary>
    /// 请求的声明
    /// </summary>
    public List<string> RequestedClaims { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public StarshineClaimsServiceOptions()
    {
        RequestedClaims = [];
    }
}
