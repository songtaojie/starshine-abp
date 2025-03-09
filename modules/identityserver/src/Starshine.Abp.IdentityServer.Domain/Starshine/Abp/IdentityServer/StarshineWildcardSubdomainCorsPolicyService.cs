using Starshine.IdentityServer.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Text.Formatting;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// 通配符子域CorsPolicyService
/// </summary>
public class StarshineWildcardSubdomainCorsPolicyService : StarshineCorsPolicyService
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="hybridServiceScopeFactory"></param>
    /// <param name="options"></param>
    public StarshineWildcardSubdomainCorsPolicyService(
        IDistributedCache<AllowedCorsOriginsCacheItem> cache,
        IServiceScopeFactory hybridServiceScopeFactory,
        IOptions<IdentityServerOptions> options)
            : base(cache, hybridServiceScopeFactory, options)
    {

    }
    /// <summary>
    /// 判断是否允许跨域
    /// </summary>
    /// <param name="allowedOrigins"></param>
    /// <param name="origin"></param>
    /// <returns></returns>
    protected override async Task<bool> IsOriginAllowedAsync(string[] allowedOrigins, string origin)
    {
        var isAllowed = await base.IsOriginAllowedAsync(allowedOrigins, origin);
        if (isAllowed)
        {
            return true;
        }

        foreach (var url in allowedOrigins)
        {
            var extractResult = FormattedStringValueExtracter.Extract(origin, url, ignoreCase: true);
            if (extractResult.IsMatch)
            {
                return extractResult.Matches.Aggregate(url, (current, nameValue) => current.Replace($"{{{nameValue.Name}}}", nameValue.Value))
                    .Contains(origin, StringComparison.OrdinalIgnoreCase);
            }

            if (url.Replace("{0}.", "").Contains(origin, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}
