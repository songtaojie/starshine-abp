using Starshine.IdentityServer.Configuration;
using Starshine.IdentityServer.Models;
using Starshine.IdentityServer.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.ObjectMapping;
using Starshine.Abp.IdentityServer.Repositories;

namespace Starshine.Abp.IdentityServer.Stores;
/// <summary>
/// ��Դ�洢
/// </summary>
public class ResourceStore : IResourceStore
{
    /// <summary>
    /// ������Դkey
    /// </summary>
    public const string AllResourcesKey = "AllResources";
    /// <summary>
    /// ��Դkey
    /// </summary>
    public const string ApiResourceNameCacheKeyPrefix = "ApiResourceName_";
    /// <summary>
    /// Api ��Դ��Χ���ƻ����ǰ׺
    /// </summary>
    public const string ApiResourceScopeNameCacheKeyPrefix = "ApiResourceScopeName_";
    /// <summary>
    /// �����Դ�ִ�
    /// </summary>
    protected IIdentityResourceRepository IdentityResourceRepository { get; }
    /// <summary>
    /// Api ��Դ�ִ�
    /// </summary>
    protected IApiResourceRepository ApiResourceRepository { get; }

    /// <summary>
    /// ApiScope�ִ�
    /// </summary>
    protected IApiScopeRepository ApiScopeRepository { get; }

    /// <summary>
    /// �����Դ����
    /// </summary>
    protected IDistributedCache<Starshine.IdentityServer.Models.IdentityResource> IdentityResourceCache { get; }

    /// <summary>
    /// ApiScope����
    /// </summary>
    protected IDistributedCache<Starshine.IdentityServer.Models.ApiScope> ApiScopeCache { get; }

    /// <summary>
    /// Api ��Դ����
    /// </summary>
    protected IDistributedCache<Starshine.IdentityServer.Models.ApiResource> ApiResourceCache { get; }

    /// <summary>
    /// Api ��Դ����
    /// </summary>
    protected IDistributedCache<IEnumerable<Starshine.IdentityServer.Models.ApiResource>> ApiResourcesCache { get; }
    
    /// <summary>
    /// ��Դ����
    /// </summary>
    protected IDistributedCache<Resources> ResourcesCache { get; }

    /// <summary>
    /// IdentityServer ѡ��
    /// </summary>
    protected IdentityServerOptions Options { get; }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="identityResourceRepository"></param>
    /// <param name="apiResourceRepository"></param>
    /// <param name="apiScopeRepository"></param>
    /// <param name="identityResourceCache"></param>
    /// <param name="apiScopeCache"></param>
    /// <param name="apiResourceCache"></param>
    /// <param name="apiResourcesCache"></param>
    /// <param name="resourcesCache"></param>
    /// <param name="options"></param>
    public ResourceStore(
        IIdentityResourceRepository identityResourceRepository,
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
    /// ͨ����Χ���ƻ�ȡ�����Դ��
    /// </summary>
    public virtual async Task<IEnumerable<Starshine.IdentityServer.Models.IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return (await GetCacheItemsAsync(IdentityResourceCache,scopeNames,
            async keys =>
            {
               var identityResources = await IdentityResourceRepository.GetListByScopeNameAsync(keys, includeDetails: true);
                return identityResources.ToIdentityResourceModel();
            },
            (models, cacheKeyPrefix) => new List<IEnumerable<KeyValuePair<string, Starshine.IdentityServer.Models.IdentityResource>>>
            {
                models.Select(x => new KeyValuePair<string, Starshine.IdentityServer.Models.IdentityResource>(AddCachePrefix(x.Name, cacheKeyPrefix), x))
            })).DistinctBy(x => x.Name);
    }

    /// <summary>
    /// ͨ����Χ���ƻ�ȡ API ��Χ��
    /// </summary>
    public virtual async Task<IEnumerable<Starshine.IdentityServer.Models.ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        return (await GetCacheItemsAsync(ApiScopeCache,scopeNames,
            async keys =>
            {
               var apiScopes = await ApiScopeRepository.GetListByNameAsync(keys, includeDetails: true);
                return apiScopes.ToApiScopeModel();
            },
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
        var apiResources = await ApiResourceRepository.GetListByScopesAsync(otherKeys, includeDetails: true);
        var otherModels = apiResources.ToApiResourceModel();
        var otherCacheItems = otherKeys.Select(otherKey => new KeyValuePair<string, IEnumerable<Starshine.IdentityServer.Models.ApiResource>>(AddCachePrefix(otherKey, ApiResourceScopeNameCacheKeyPrefix), otherModels)).ToList();
        await ApiResourcesCache.SetManyAsync(otherCacheItems, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
        });

        return cacheItems.Where(x => x.Value != null).SelectMany(x => x.Value!).Concat(otherModels).DistinctBy(x => x.Name);
    }

    /// <summary>
    /// ͨ��API��Դ���ƻ�ȡAPI��Դ��
    /// </summary>
    public virtual async Task<IEnumerable<Starshine.IdentityServer.Models.ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        return (await GetCacheItemsAsync(ApiResourceCache,apiResourceNames,
            async keys =>
            {
                var apiResources = await ApiResourceRepository.FindByNameAsync(keys, includeDetails: true);
                return apiResources.ToApiResourceModel();
            },
            (models, cacheKeyPrefix) => new List<IEnumerable<KeyValuePair<string, Starshine.IdentityServer.Models.ApiResource>>>
            {
                    models.Select(x => new KeyValuePair<string, Starshine.IdentityServer.Models.ApiResource>(AddCachePrefix(x.Name, cacheKeyPrefix), x))
            }, ApiResourceNameCacheKeyPrefix)).DistinctBy(x => x.Name);
    }

    /// <summary>
    /// ��ȡ������Դ��
    /// </summary>
    public virtual async Task<Resources?> GetAllResourcesAsync()
    {
        return await ResourcesCache.GetOrAddAsync(AllResourcesKey, async () =>
        {
            var identityResources = await IdentityResourceRepository.GetListAsync(includeDetails: true);
            var apiResources = await ApiResourceRepository.GetListAsync(includeDetails: true);
            var apiScopes = await ApiScopeRepository.GetListAsync(includeDetails: true);

            return new Resources(
                identityResources.ToIdentityResourceModel(),
                apiResources.ToApiResourceModel(),
                apiScopes.ToApiScopeModel());
        }, () => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
        });
    }

    /// <summary>
    /// ��ȡ�����
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="cache"></param>
    /// <param name="keys"></param>
    /// <param name="entityFactory"></param>
    /// <param name="cacheItemsFactory"></param>
    /// <param name="cacheKeyPrefix"></param>
    /// <returns></returns>
    protected virtual async Task<IEnumerable<TModel>> GetCacheItemsAsync<TModel>(
        IDistributedCache<TModel> cache,
        IEnumerable<string> keys,
        Func<string[], Task<List<TModel>>> entityFactory,
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
        var otherModels = await entityFactory(otherKeys);
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

    /// <summary>
    /// ��ӻ���ǰ׺��
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    protected virtual IEnumerable<string> AddCachePrefix(IEnumerable<string> keys, string? prefix)
    {
        return prefix == null ? keys : keys.Select(x => AddCachePrefix(x, prefix));
    }

    /// <summary>
    /// ��ӻ���ǰ׺��
    /// </summary>
    /// <param name="key"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    protected virtual string AddCachePrefix(string key, string? prefix)
    {
        return prefix == null ? key : prefix + key;
    }

    /// <summary>
    /// �Ƴ�����ǰ׺��
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    protected virtual IEnumerable<string> RemoveCachePrefix(IEnumerable<string> keys, string? prefix)
    {
        return prefix == null ? keys : keys.Select(x => RemoveCachePrefix(x, prefix));
    }

    /// <summary>
    /// �Ƴ�����ǰ׺��
    /// </summary>
    /// <param name="key"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    protected virtual string RemoveCachePrefix(string key, string prefix)
    {
        return prefix == null ? key : key.RemovePreFix(prefix);
    }
}
