using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Starshine.IdentityServer.Configuration;
using Starshine.IdentityServer.Models;
using Starshine.IdentityServer.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Starshine.Abp.IdentityServer.ApiResources;
using Starshine.Abp.IdentityServer.ApiScopes;
using Starshine.Abp.IdentityServer.IdentityResources;
using Volo.Abp.ObjectMapping;
using ApiResource = Starshine.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Starshine.Abp.IdentityServer.ApiScopes.ApiScope;
using IdentityResource = Starshine.Abp.IdentityServer.IdentityResources.IdentityResource;

namespace Starshine.Abp.IdentityServer;

public class ResourceStore : IResourceStore
{
    public const string AllResourcesKey = "AllResources";
    public const string ApiResourceNameCacheKeyPrefix = "ApiResourceName_";
    public const string ApiResourceScopeNameCacheKeyPrefix = "ApiResourceScopeName_";

    protected IIdentityResourceRepository IdentityResourceRepository { get; }
    protected IApiResourceRepository ApiResourceRepository { get; }
    protected IApiScopeRepository ApiScopeRepository { get; }
    protected IObjectMapper<StarshineIdentityServerDomainModule> ObjectMapper { get; }
    protected IDistributedCache<Starshine.IdentityServer.Models.IdentityResource> IdentityResourceCache { get; }
    protected IDistributedCache<Starshine.IdentityServer.Models.ApiScope> ApiScopeCache { get; }
    protected IDistributedCache<Starshine.IdentityServer.Models.ApiResource> ApiResourceCache { get; }
    protected IDistributedCache<IEnumerable<Starshine.IdentityServer.Models.ApiResource>> ApiResourcesCache { get; }
    protected IDistributedCache<Starshine.IdentityServer.Models.Resources> ResourcesCache { get; }
    protected IdentityServerOptions Options { get; }

    public ResourceStore(
        IIdentityResourceRepository identityResourceRepository,
        IObjectMapper<StarshineIdentityServerDomainModule> objectMapper,
        IApiResourceRepository apiResourceRepository,
        IApiScopeRepository apiScopeRepository,
        IDistributedCache<Starshine.IdentityServer.Models.IdentityResource> identityResourceCache,
        IDistributedCache<Starshine.IdentityServer.Models.ApiScope> apiScopeCache,
        IDistributedCache<Starshine.IdentityServer.Models.ApiResource> apiResourceCache,
        IDistributedCache<IEnumerable<Starshine.IdentityServer.Models.ApiResource>> apiResourcesCache,
        IDistributedCache<Resources> resourcesCache,
        IOptions<IdentityServerOptions> options)
    {
        IdentityResourceRepository = identityResourceRepository;
        ObjectMapper = objectMapper;
        ApiResourceRepository = apiResourceRepository;
        ApiScopeRepository = apiScopeRepository;
        IdentityResourceCache = identityResourceCache;
        ApiScopeCache = apiScopeCache;
        ApiResourceCache = apiResourceCache;
        ApiResourcesCache = apiResourcesCache;
        ResourcesCache = resourcesCache;
        Options = options.Value;
    }

    /// <summary>
    /// Gets identity resources by scope name.
    /// </summary>
    public virtual async Task<IEnumerable<Starshine.IdentityServer.Models.IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return (await GetCacheItemsAsync(
            IdentityResourceCache,
            scopeNames,
            async keys => await IdentityResourceRepository.GetListByScopeNameAsync(keys, includeDetails: true),
            (models, cacheKeyPrefix) => new List<IEnumerable<KeyValuePair<string, Starshine.IdentityServer.Models.IdentityResource>>>
            {
                    models.Select(x => new KeyValuePair<string, Starshine.IdentityServer.Models.IdentityResource>(AddCachePrefix(x.Name, cacheKeyPrefix), x))
            })).DistinctBy(x => x.Name);
    }

    /// <summary>
    /// Gets API scopes by scope name.
    /// </summary>
    public virtual async Task<IEnumerable<Starshine.IdentityServer.Models.ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        return (await GetCacheItemsAsync(
            ApiScopeCache,
            scopeNames,
            async keys => await ApiScopeRepository.GetListByNameAsync(keys, includeDetails: true),
            (models, cacheKeyPrefix) => new List<IEnumerable<KeyValuePair<string, Starshine.IdentityServer.Models.ApiScope>>>
            {
                    models.Select(x => new KeyValuePair<string, Starshine.IdentityServer.Models.ApiScope>(AddCachePrefix(x.Name, cacheKeyPrefix), x))
            })).DistinctBy(x => x.Name);
    }

    /// <summary>
    /// Gets API resources by scope name.
    /// </summary>
    public virtual async Task<IEnumerable<Starshine.IdentityServer.Models.ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        var cacheItems = await ApiResourcesCache.GetManyAsync(AddCachePrefix(scopeNames, ApiResourceScopeNameCacheKeyPrefix));
        if (cacheItems.All(x => x.Value != null))
        {
            return cacheItems.SelectMany(x => x.Value!).DistinctBy(x => x.Name);
        }

        var otherKeys = RemoveCachePrefix(cacheItems.Where(x => x.Value == null).Select(x => x.Key), ApiResourceScopeNameCacheKeyPrefix).ToArray();
        var otherModels = ObjectMapper.Map<List<ApiResource>, List<Starshine.IdentityServer.Models.ApiResource>>(await ApiResourceRepository.GetListByScopesAsync(otherKeys, includeDetails: true));

        var otherCacheItems = otherKeys.Select(otherKey => new KeyValuePair<string, IEnumerable<Starshine.IdentityServer.Models.ApiResource>>(AddCachePrefix(otherKey, ApiResourceScopeNameCacheKeyPrefix), otherModels)).ToList();
        await ApiResourcesCache.SetManyAsync(otherCacheItems, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
        });

        return cacheItems.Where(x => x.Value != null).SelectMany(x => x.Value!).Concat(otherModels).DistinctBy(x => x.Name);
    }

    /// <summary>
    /// Gets API resources by API resource name.
    /// </summary>
    public virtual async Task<IEnumerable<Starshine.IdentityServer.Models.ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        return (await GetCacheItemsAsync(
            ApiResourceCache,
            apiResourceNames,
            async keys => await ApiResourceRepository.FindByNameAsync(keys, includeDetails: true),
            (models, cacheKeyPrefix) => new List<IEnumerable<KeyValuePair<string, Starshine.IdentityServer.Models.ApiResource>>>
            {
                    models.Select(x => new KeyValuePair<string, Starshine.IdentityServer.Models.ApiResource>(AddCachePrefix(x.Name, cacheKeyPrefix), x))
            }, ApiResourceNameCacheKeyPrefix)).DistinctBy(x => x.Name);
    }

    /// <summary>
    /// Gets all resources.
    /// </summary>
    public virtual async Task<Starshine.IdentityServer.Models.Resources?> GetAllResourcesAsync()
    {
        return await ResourcesCache.GetOrAddAsync(AllResourcesKey, async () =>
        {
            var identityResources = await IdentityResourceRepository.GetListAsync(includeDetails: true);
            var apiResources = await ApiResourceRepository.GetListAsync(includeDetails: true);
            var apiScopes = await ApiScopeRepository.GetListAsync(includeDetails: true);

            return new Resources(
                ObjectMapper.Map<List<Starshine.Abp.IdentityServer.IdentityResources.IdentityResource>, List<Starshine.IdentityServer.Models.IdentityResource>>(identityResources),
                ObjectMapper.Map<List<Starshine.Abp.IdentityServer.ApiResources.ApiResource>, List<Starshine.IdentityServer.Models.ApiResource>>(apiResources),
                ObjectMapper.Map<List<Starshine.Abp.IdentityServer.ApiScopes.ApiScope>, List<Starshine.IdentityServer.Models.ApiScope>>(apiScopes));
        }, () => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
        });
    }

    protected virtual async Task<IEnumerable<TModel>> GetCacheItemsAsync<TEntity, TModel>(
        IDistributedCache<TModel> cache,
        IEnumerable<string> keys,
        Func<string[], Task<List<TEntity>>> entityFactory,
        Func<List<TModel>, string?, List<IEnumerable<KeyValuePair<string, TModel>>>> cacheItemsFactory,
        string? cacheKeyPrefix = null)
        where TModel : class
    {
        var cacheItems = await cache.GetManyAsync(AddCachePrefix(keys, cacheKeyPrefix));
        if (cacheItems.All(x => x.Value != null))
        {
            return cacheItems.Select(x => x.Value!);
        }

        var otherKeys = RemoveCachePrefix(cacheItems.Where(x => x.Value == null).Select(x => x.Key), cacheKeyPrefix).ToArray();
        var otherModels = ObjectMapper.Map<List<TEntity>, List<TModel>>(await entityFactory(otherKeys));
        var otherCacheItems = cacheItemsFactory(otherModels, cacheKeyPrefix).ToList();
        foreach (var item in otherCacheItems)
        {
            await cache.SetManyAsync(item, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
            });
        }

        return cacheItems.Where(x => x.Value != null).Select(x => x.Value!).Concat(otherModels);
    }

    protected virtual IEnumerable<string> AddCachePrefix(IEnumerable<string> keys, string? prefix)
    {
        return prefix == null ? keys : keys.Select(x => AddCachePrefix(x, prefix));
    }

    protected virtual string AddCachePrefix(string key, string? prefix)
    {
        return prefix == null ? key : prefix + key;
    }

    protected virtual IEnumerable<string> RemoveCachePrefix(IEnumerable<string> keys, string? prefix)
    {
        return prefix == null ? keys : keys.Select(x => RemoveCachePrefix(x, prefix));
    }

    protected virtual string RemoveCachePrefix(string key, string prefix)
    {
        return prefix == null ? key : key.RemovePreFix(prefix);
    }
}
