using Starshine.IdentityServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Starshine.IdentityServer.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Starshine.Abp.IdentityServer.Repositories;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// Cors 政策服务
/// </summary>
public class StarshineCorsPolicyService : ICorsPolicyService
{
    /// <summary>
    /// Logger
    /// </summary>
    public ILogger<StarshineCorsPolicyService> Logger { get; set; }
    /// <summary>
    /// 服务作用域工厂
    /// </summary>
    protected IServiceScopeFactory HybridServiceScopeFactory { get; }
    /// <summary>
    /// 缓存
    /// </summary>
    protected IDistributedCache<AllowedCorsOriginsCacheItem> Cache { get; }
    /// <summary>
    /// 身份服务选项
    /// </summary>
    protected IdentityServerOptions Options { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="hybridServiceScopeFactory"></param>
    /// <param name="options"></param>
    public StarshineCorsPolicyService(
        IDistributedCache<AllowedCorsOriginsCacheItem> cache,
        IServiceScopeFactory hybridServiceScopeFactory,
        IOptions<IdentityServerOptions> options)
    {
        Cache = cache;
        HybridServiceScopeFactory = hybridServiceScopeFactory;
        Options = options.Value;
        Logger = NullLogger<StarshineCorsPolicyService>.Instance;
    }

    /// <summary>
    /// 判断是否允许跨域
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public virtual async Task<bool> IsOriginAllowedAsync(string origin)
    {
        var cacheItem = await Cache.GetOrAddAsync(AllowedCorsOriginsCacheItem.AllOrigins, CreateCacheItemAsync,
            () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = Options.Caching.CorsExpiration
            });

        var isAllowed = await IsOriginAllowedAsync(cacheItem!.AllowedOrigins, origin);

        if (!isAllowed)
        {
            Logger.LogWarning($"Origin is not allowed: {origin}");
        }

        return isAllowed;
    }

    /// <summary>
    /// 创建缓存项
    /// </summary>
    /// <returns></returns>
    protected virtual async Task<AllowedCorsOriginsCacheItem> CreateCacheItemAsync()
    {
        // doing this here and not in the ctor because: https://github.com/aspnet/AspNetCore/issues/2377
        using var scope = HybridServiceScopeFactory.CreateScope();
        var clientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

        return new AllowedCorsOriginsCacheItem
        {
            AllowedOrigins = (await clientRepository.GetAllDistinctAllowedCorsOriginsAsync()).ToArray()
        };
    }

    /// <summary>
    /// 判断是否允许跨域
    /// </summary>
    /// <param name="allowedOrigins"></param>
    /// <param name="origin"></param>
    /// <returns></returns>
    protected virtual Task<bool> IsOriginAllowedAsync(string[] allowedOrigins, string origin)
    {
        if (allowedOrigins == null) return Task.FromResult(false);
        return Task.FromResult(allowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase));
    }
}
