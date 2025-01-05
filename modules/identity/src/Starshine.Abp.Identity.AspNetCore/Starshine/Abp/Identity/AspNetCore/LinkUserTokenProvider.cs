using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Starshine.Abp.Identity.AspNetCore;
/// <summary>
/// 链接用户令牌提供者
/// </summary>
public class LinkUserTokenProvider : DataProtectorTokenProvider<IdentityUser>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataProtectionProvider"></param>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    public LinkUserTokenProvider(
        IDataProtectionProvider dataProtectionProvider,
        IOptions<DataProtectionTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<IdentityUser>> logger)
        : base(dataProtectionProvider, options, logger)
    {

    }
}
