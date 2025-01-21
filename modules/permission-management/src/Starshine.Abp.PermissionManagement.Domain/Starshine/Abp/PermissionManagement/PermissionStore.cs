using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限存储
/// </summary>
public class PermissionStore : IPermissionStore, ITransientDependency
{
    /// <summary>
    /// 日志记录
    /// </summary>
    protected ILogger<PermissionStore> Logger { get; }

    /// <summary>
    /// 
    /// </summary>
    protected IPermissionGrantRepository PermissionGrantRepository { get; }

    /// <summary>
    /// 
    /// </summary>
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    /// <summary>
    /// 
    /// </summary>
    protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionGrantRepository"></param>
    /// <param name="cache"></param>
    /// <param name="permissionDefinitionManager"></param>
    public PermissionStore(
        IPermissionGrantRepository permissionGrantRepository,
        IDistributedCache<PermissionGrantCacheItem> cache,
        IPermissionDefinitionManager permissionDefinitionManager)
    {
        PermissionGrantRepository = permissionGrantRepository;
        Cache = cache;
        PermissionDefinitionManager = permissionDefinitionManager;
        Logger = NullLogger<PermissionStore>.Instance;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
    {
        return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
    }

    /// <summary>
    /// 获取权限缓存项
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(string name,string providerName,string providerKey)
    {
        var cacheKey = CalculateCacheKey(name, providerName, providerKey);
        Logger.LogDebug($"权限存储.GetCacheItemAsync: {cacheKey}");
        var cacheItem = await Cache.GetAsync(cacheKey);

        if (cacheItem != null)
        {
            Logger.LogDebug($"在缓存中找到: {cacheKey}");
            return cacheItem;
        }
        Logger.LogDebug($"缓存中未找到: {cacheKey}");
        cacheItem = new PermissionGrantCacheItem(false);
        await SetCacheItemsAsync(providerName, providerKey, name, cacheItem);
        return cacheItem;
    }

    /// <summary>
    /// 设置权限缓存项
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="currentName"></param>
    /// <param name="currentCacheItem"></param>
    /// <returns></returns>
    protected virtual async Task SetCacheItemsAsync(string providerName,string providerKey,string currentName, PermissionGrantCacheItem currentCacheItem)
    {
        var permissions = await PermissionDefinitionManager.GetPermissionsAsync();

        Logger.LogDebug($"从存储库中获取此提供商名称的所有已授予的权限,key: {providerName},{providerKey}");

        var grantedPermissionsHashSet = new HashSet<string>(
            (await PermissionGrantRepository.GetListAsync(providerName, providerKey)).Select(p => p.Name)
        );

        Logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

        var cacheItems = new List<KeyValuePair<string, PermissionGrantCacheItem>>();

        foreach (var permission in permissions)
        {
            var isGranted = grantedPermissionsHashSet.Contains(permission.Name);

            cacheItems.Add(new KeyValuePair<string, PermissionGrantCacheItem>(
                CalculateCacheKey(permission.Name, providerName, providerKey),
                new PermissionGrantCacheItem(isGranted))
            );

            if (permission.Name == currentName)
            {
                currentCacheItem.IsGranted = isGranted;
            }
        }

        await Cache.SetManyAsync(cacheItems);

        Logger.LogDebug($"完成缓存项的设置. 数量: {permissions.Count}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="names"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        Check.NotNullOrEmpty(names, nameof(names));

        var result = new MultiplePermissionGrantResult();

        if (names.Length == 1)
        {
            var name = names.First();
            result.Result.Add(name,
                await IsGrantedAsync(names.First(), providerName, providerKey)
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined);
            return result;
        }

        var cacheItems = await GetCacheItemsAsync(names, providerName, providerKey);
        foreach (var item in cacheItems)
        {
            result.Result.Add(GetPermissionNameFormCacheKeyOrNull(item.Key)!,
                item.Value != null && item.Value.IsGranted
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined);
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="names"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected virtual async Task<List<KeyValuePair<string, PermissionGrantCacheItem>>> GetCacheItemsAsync(
        string[] names,
        string providerName,
        string providerKey)
    {
        var cacheKeys = names.Select(x => CalculateCacheKey(x, providerName, providerKey)).ToList();

        Logger.LogDebug($"权限存储.GetCacheItemAsync: {string.Join(",", cacheKeys)}");

        var cacheItems = (await Cache.GetManyAsync(cacheKeys)).ToList();
        if (cacheItems.All(x => x.Value != null))
        {
            Logger.LogDebug($"在缓存中找到: {string.Join(",", cacheKeys)}");
            return cacheItems!;
        }

        var notCacheKeys = cacheItems.Where(x => x.Value == null).Select(x => x.Key).ToList();

        Logger.LogDebug($"缓存中未找到: {string.Join(",", notCacheKeys)}");

        var newCacheItems = await SetCacheItemsAsync(providerName, providerKey, notCacheKeys);

        var result = new List<KeyValuePair<string, PermissionGrantCacheItem>>();
        foreach (var key in cacheKeys)
        {
            var item = newCacheItems.FirstOrDefault(x => x.Key == key);
            if (item.Value == null)
            {
                item = cacheItems.FirstOrDefault(x => x.Key == key)!;
            }

            result.Add(new KeyValuePair<string, PermissionGrantCacheItem>(key, item.Value));
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="notCacheKeys"></param>
    /// <returns></returns>
    protected virtual async Task<List<KeyValuePair<string, PermissionGrantCacheItem>>> SetCacheItemsAsync(
        string providerName,
        string providerKey,
        List<string> notCacheKeys)
    {
        var permissions = (await PermissionDefinitionManager.GetPermissionsAsync())
            .Where(x => notCacheKeys.Any(k => GetPermissionNameFormCacheKeyOrNull(k) == x.Name)).ToList();
        Logger.LogDebug($"从存储库中获取此提供商名称未缓存授予的权限,key: {providerName},{providerKey}");
        var grantedPermissionsHashSet = new HashSet<string>(
            (await PermissionGrantRepository.GetListAsync(notCacheKeys.Select(GetPermissionNameFormCacheKeyOrNull).ToArray()!, providerName, providerKey)).Select(p => p.Name)
        );
        Logger.LogDebug($"设置缓存项. 数量: {permissions.Count}");

        var cacheItems = new List<KeyValuePair<string, PermissionGrantCacheItem>>();

        foreach (var permission in permissions)
        {
            var isGranted = grantedPermissionsHashSet.Contains(permission.Name);

            cacheItems.Add(new KeyValuePair<string, PermissionGrantCacheItem>(
                CalculateCacheKey(permission.Name, providerName, providerKey),
                new PermissionGrantCacheItem(isGranted))
            );
        }

        await Cache.SetManyAsync(cacheItems);
        Logger.LogDebug($"完成缓存项的设置. 数量: {permissions.Count}");
        return cacheItems;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
    {
        return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected virtual string? GetPermissionNameFormCacheKeyOrNull(string key)
    {
        //TODO: 当名称为空时抛出 ex？
        return PermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull(key);
    }
}
