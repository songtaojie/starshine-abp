using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Starshine.IdentityServer.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Volo.Abp.Security.Claims;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// 声明服务
/// </summary>
public class StarshineClaimsService : DefaultClaimsService
{
    /// <summary>
    /// 声明服务选项
    /// </summary>
    protected readonly StarshineClaimsServiceOptions Options;

    private static readonly string[] AdditionalOptionalClaimNames =
    {
        AbpClaimTypes.TenantId,
        AbpClaimTypes.ImpersonatorTenantId,
        AbpClaimTypes.ImpersonatorUserId,
        AbpClaimTypes.Name,
        AbpClaimTypes.SurName,
        JwtRegisteredClaimNames.UniqueName,
        JwtClaimTypes.PreferredUserName,
        JwtClaimTypes.GivenName,
        JwtClaimTypes.FamilyName,
    };

    /// <summary>
    /// 声明服务
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public StarshineClaimsService(
        IProfileService profile,
        ILogger<DefaultClaimsService> logger,
        IOptions<StarshineClaimsServiceOptions> options)
        : base(profile, logger)
    {
        Options = options.Value;
    }

    /// <summary>
    /// 过滤请求的声明类型
    /// </summary>
    /// <param name="claimTypes"></param>
    /// <returns></returns>
    protected override IEnumerable<string> FilterRequestedClaimTypes(IEnumerable<string> claimTypes)
    {
        return base.FilterRequestedClaimTypes(claimTypes)
            .Union(Options.RequestedClaims);
    }

    /// <summary>
    /// 获取可选的声明
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    protected override IEnumerable<Claim> GetOptionalClaims(ClaimsPrincipal subject)
    {
        return base.GetOptionalClaims(subject)
            .Union(GetAdditionalOptionalClaims(subject));
    }

    /// <summary>
    /// 获取可选的声明
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    protected virtual IEnumerable<Claim> GetAdditionalOptionalClaims(ClaimsPrincipal subject)
    {
        foreach (var claimName in AdditionalOptionalClaimNames)
        {
            var claim = subject.FindFirst(claimName);
            if (claim != null)
            {
                yield return claim;
            }
        }
    }
}
