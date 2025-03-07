using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Starshine.Abp.IdentityServer.Clients;
using Starshine.Abp.IdentityServer.ApiScopes;
using Starshine.Abp.IdentityServer.IdentityResources;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer;

public class IdentityServerCacheItemInvalidator :
    ILocalEventHandler<EntityChangedEventData<Client>>,
    ILocalEventHandler<EntityChangedEventData<ClientCorsOrigin>>,
    ILocalEventHandler<EntityChangedEventData<IdentityResource>>,
    ILocalEventHandler<EntityChangedEventData<ApiResource>>,
    ILocalEventHandler<EntityChangedEventData<ApiScope>>,
    ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public IdentityServerCacheItemInvalidator(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<Client> eventData)
    {
        var clientCache = ServiceProvider.GetRequiredService<IDistributedCache<Starshine.IdentityServer.Models.Client>>();
        await clientCache.RemoveAsync(eventData.Entity.ClientId, considerUow: true);

        var corsCache = ServiceProvider.GetRequiredService<IDistributedCache<AllowedCorsOriginsCacheItem>>();
        await corsCache.RemoveAsync(AllowedCorsOriginsCacheItem.AllOrigins);
    }

    public async Task HandleEventAsync(EntityChangedEventData<ClientCorsOrigin> eventData)
    {
        var corsCache = ServiceProvider.GetRequiredService<IDistributedCache<AllowedCorsOriginsCacheItem>>();
        await corsCache.RemoveAsync(AllowedCorsOriginsCacheItem.AllOrigins);
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<IdentityResource> eventData)
    {
        var cache = ServiceProvider.GetRequiredService<IDistributedCache<Starshine.IdentityServer.Models.IdentityResource>>();
        await cache.RemoveAsync(eventData.Entity.Name);

        var resourcesCache = ServiceProvider.GetRequiredService<IDistributedCache<Starshine.IdentityServer.Models.Resources>>();
        await resourcesCache.RemoveAsync(ResourceStore.AllResourcesKey);
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<ApiResource> eventData)
    {
        var cache = ServiceProvider.GetRequiredService<IDistributedCache<Starshine.IdentityServer.Models.ApiResource>>();
        await cache.RemoveAsync(ResourceStore.ApiResourceNameCacheKeyPrefix + eventData.Entity.Name);
        await cache.RemoveManyAsync(eventData.Entity.Scopes.Select(x => ResourceStore.ApiResourceScopeNameCacheKeyPrefix + x.Scope));

        var resourcesCache = ServiceProvider.GetRequiredService<IDistributedCache<Starshine.IdentityServer.Models.Resources>>();
        await resourcesCache.RemoveAsync(ResourceStore.AllResourcesKey);
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<ApiScope> eventData)
    {
        var cache = ServiceProvider.GetRequiredService<IDistributedCache<Starshine.IdentityServer.Models.ApiScope>>();
        await cache.RemoveAsync(eventData.Entity.Name);

        var resourcesCache = ServiceProvider.GetRequiredService<IDistributedCache<Starshine.IdentityServer.Models.Resources>>();
        await resourcesCache.RemoveAsync(ResourceStore.AllResourcesKey);
    }
}
