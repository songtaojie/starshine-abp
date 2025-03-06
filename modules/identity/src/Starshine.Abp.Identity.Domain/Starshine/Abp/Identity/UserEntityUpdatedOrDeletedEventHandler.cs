using Microsoft.Extensions.Logging;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Starshine.Abp.Identity;

/// <summary>
/// 用户实体更新或删除事件处理程序
/// </summary>
public class UserEntityUpdatedOrDeletedEventHandler :
    ILocalEventHandler<EntityUpdatedEventData<IdentityUser>>,
    ILocalEventHandler<EntityDeletedEventData<IdentityUser>>,
    ITransientDependency
{
    
    private readonly ILogger<UserEntityUpdatedOrDeletedEventHandler> _logger;

    private readonly IDistributedCache<AbpDynamicClaimCacheItem> _dynamicClaimCache;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dynamicClaimCache"></param>
    /// <param name="logger"></param>
    public UserEntityUpdatedOrDeletedEventHandler(IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache,
        ILogger<UserEntityUpdatedOrDeletedEventHandler> logger)
    {
        _logger = logger;
        _dynamicClaimCache = dynamicClaimCache;
    }

    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityUpdatedEventData<IdentityUser> eventData)
    {
        await RemoveDynamicClaimCacheAsync(eventData.Entity.Id, eventData.Entity.TenantId);
    }

    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<IdentityUser> eventData)
    {
        await RemoveDynamicClaimCacheAsync(eventData.Entity.Id, eventData.Entity.TenantId);
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    protected virtual async Task RemoveDynamicClaimCacheAsync(Guid userId, Guid? tenantId)
    {
        _logger.LogDebug($"Remove dynamic claims cache for user: {userId}");
        await _dynamicClaimCache.RemoveAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId));
    }
}
