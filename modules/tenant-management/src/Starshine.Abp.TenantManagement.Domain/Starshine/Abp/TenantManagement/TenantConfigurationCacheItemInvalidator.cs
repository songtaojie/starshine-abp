using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.TenantManagement;

/// <summary>
/// �⻧���û�������֤��
/// </summary>
[LocalEventHandlerOrder(-1)]
public class TenantConfigurationCacheItemInvalidator :
    ILocalEventHandler<EntityChangedEventData<Tenant>>,
    ILocalEventHandler<TenantChangedEvent>,
    ITransientDependency
{
    /// <summary>
    /// �������
    /// </summary>
    protected IDistributedCache<TenantConfigurationCacheItem> Cache { get; }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="cache"></param>
    public TenantConfigurationCacheItemInvalidator(IDistributedCache<TenantConfigurationCacheItem> cache)
    {
        Cache = cache;
    }

    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public virtual async Task HandleEventAsync(EntityChangedEventData<Tenant> eventData)
    {
        if (eventData is EntityCreatedEventData<Tenant>)
        {
            return;
        }

        await ClearCacheAsync(eventData.Entity.Id, eventData.Entity.NormalizedName);
    }

    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public virtual async Task HandleEventAsync(TenantChangedEvent eventData)
    {
        await ClearCacheAsync(eventData.Id, eventData.NormalizedName);
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="id"></param>
    /// <param name="normalizedName"></param>
    /// <returns></returns>
    protected virtual async Task ClearCacheAsync(Guid? id, string? normalizedName)
    {
        await Cache.RemoveManyAsync(
            new[]
            {
                TenantConfigurationCacheItem.CalculateCacheKey(id, null),
                TenantConfigurationCacheItem.CalculateCacheKey(null, normalizedName),
                TenantConfigurationCacheItem.CalculateCacheKey(id, normalizedName),
            }, considerUow: true);
    }
}