using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Starshine.Abp.Identity.Managers;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份动态声明主体贡献者缓存
/// </summary>
public class IdentityDynamicClaimsPrincipalContributorCache : ITransientDependency
{
    /// <summary>
    /// 日志记录
    /// </summary>
    protected ILogger<IdentityDynamicClaimsPrincipalContributorCache> Logger { get; }
    /// <summary>
    /// 分布式缓存
    /// </summary>
    protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; }
    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }
    /// <summary>
    /// 用户管理
    /// </summary>
    protected IdentityUserManager UserManager { get; }
    /// <summary>
    /// 用户声明主体工厂
    /// </summary>
    protected IUserClaimsPrincipalFactory<IdentityUser> UserClaimsPrincipalFactory { get; }
    /// <summary>
    /// Abp 声明主要工厂期权
    /// </summary>
    protected IOptions<AbpClaimsPrincipalFactoryOptions> AbpClaimsPrincipalFactoryOptions { get; }
    /// <summary>
    /// 缓存配置
    /// </summary>
    protected IOptions<IdentityDynamicClaimsPrincipalContributorCacheOptions> CacheOptions { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dynamicClaimCache"></param>
    /// <param name="currentTenant"></param>
    /// <param name="userManager"></param>
    /// <param name="userClaimsPrincipalFactory"></param>
    /// <param name="abpClaimsPrincipalFactoryOptions"></param>
    /// <param name="cacheOptions"></param>
    /// <param name="logger"></param>
    public IdentityDynamicClaimsPrincipalContributorCache(
        IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache,
        ICurrentTenant currentTenant,
        IdentityUserManager userManager,
        IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory,
        IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions,
        IOptions<IdentityDynamicClaimsPrincipalContributorCacheOptions> cacheOptions,
        ILogger<IdentityDynamicClaimsPrincipalContributorCache> logger)
    {
        DynamicClaimCache = dynamicClaimCache;
        CurrentTenant = currentTenant;
        UserManager = userManager;
        UserClaimsPrincipalFactory = userClaimsPrincipalFactory;
        AbpClaimsPrincipalFactoryOptions = abpClaimsPrincipalFactoryOptions;
        CacheOptions = cacheOptions;

        Logger = logger;
    }

    /// <summary>
    /// 获取缓存项
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public virtual async Task<AbpDynamicClaimCacheItem?> GetAsync(Guid userId, Guid? tenantId = null)
    {
        Logger.LogDebug($"获取用户的动态声明缓存: {userId}");

        if (AbpClaimsPrincipalFactoryOptions.Value.DynamicClaims.IsNullOrEmpty())
        {
            var emptyCacheItem = new AbpDynamicClaimCacheItem();
            await DynamicClaimCache.SetAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId), emptyCacheItem, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheOptions.Value.CacheAbsoluteExpiration
            });

            return emptyCacheItem;
        }

        return await DynamicClaimCache.GetOrAddAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId), async () =>
        {
            using (CurrentTenant.Change(tenantId))
            {
                Logger.LogDebug($"为用户: {userId}填充动态声明缓存");

                var user = await UserManager.GetByIdAsync(userId);
                var principal = await UserClaimsPrincipalFactory.CreateAsync(user);

                var dynamicClaims = new AbpDynamicClaimCacheItem();
                foreach (var claimType in AbpClaimsPrincipalFactoryOptions.Value.DynamicClaims)
                {
                    var claims = principal.Claims.Where(x => x.Type == claimType).ToList();
                    if (claims.Count != 0)
                    {
                        dynamicClaims.Claims.AddRange(claims.Select(claim => new AbpDynamicClaim(claimType, claim.Value)));
                    }
                    else
                    {
                        dynamicClaims.Claims.Add(new AbpDynamicClaim(claimType, null));
                    }
                }

                return dynamicClaims;
            }
        }, () => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheOptions.Value.CacheAbsoluteExpiration
        });
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public virtual async Task ClearAsync(Guid userId, Guid? tenantId = null)
    {
        Logger.LogDebug($"删除用户: {userId}的动态声明缓存");
        await DynamicClaimCache.RemoveAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId));
    }
}
