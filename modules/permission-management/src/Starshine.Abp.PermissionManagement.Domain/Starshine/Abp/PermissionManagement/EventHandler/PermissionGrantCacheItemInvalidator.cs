using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限授予缓存项无效器本地事件处理程序
/// </summary>
public class PermissionGrantCacheItemInvalidator :ILocalEventHandler<EntityChangedEventData<PermissionGrant>>,ITransientDependency
{
    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }

    /// <summary>
    /// 分布式缓存
    /// </summary>
    protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cache">分布式缓存</param>
    /// <param name="currentTenant">当前租户</param>
    public PermissionGrantCacheItemInvalidator(IDistributedCache<PermissionGrantCacheItem> cache, ICurrentTenant currentTenant)
    {
        Cache = cache;
        CurrentTenant = currentTenant;
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public virtual async Task HandleEventAsync(EntityChangedEventData<PermissionGrant> eventData)
    {
        var cacheKey = CalculateCacheKey(eventData.Entity.Name, eventData.Entity.ProviderName,eventData.Entity.ProviderKey);

        using (CurrentTenant.Change(eventData.Entity.TenantId))
        {
            await Cache.RemoveAsync(cacheKey, considerUow: true);
        }
    }

    /// <summary>
    /// 获取缓存的key
    /// </summary>
    /// <param name="name">权限名称</param>
    /// <param name="providerName">提供者名称</param>
    /// <param name="providerKey">提供者key</param>
    /// <returns></returns>
    protected virtual string CalculateCacheKey(string name, string providerName, string? providerKey)
    {
        return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
    }
}
