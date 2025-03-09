using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// 缓存失效
/// </summary>
public class AllowedCorsOriginsCacheItemInvalidator : ILocalEventHandler<EntityChangedEventData<Client>>,
    ILocalEventHandler<EntityChangedEventData<ClientCorsOrigin>>, ITransientDependency
{
    /// <summary>
    /// 缓存
    /// </summary>
    protected IDistributedCache<AllowedCorsOriginsCacheItem> Cache { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cache"></param>
    public AllowedCorsOriginsCacheItemInvalidator(IDistributedCache<AllowedCorsOriginsCacheItem> cache)
    {
        Cache = cache;
    }

    /// <summary>
    /// 缓存失效
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public virtual async Task HandleEventAsync(EntityChangedEventData<Client> eventData)
    {
        await Cache.RemoveAsync(AllowedCorsOriginsCacheItem.AllOrigins);
    }

    /// <summary>
    /// 缓存失效
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public virtual async Task HandleEventAsync(EntityChangedEventData<ClientCorsOrigin> eventData)
    {
        await Cache.RemoveAsync(AllowedCorsOriginsCacheItem.AllOrigins);
    }
}
