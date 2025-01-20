using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 动态权限定义存储
/// </summary>
[Dependency(ReplaceServices = true)]
public class DynamicPermissionDefinitionStore : IDynamicPermissionDefinitionStore, ITransientDependency
{
    /// <summary>
    /// 权限组存储库
    /// </summary>
    protected IPermissionGroupDefinitionRecordRepository PermissionGroupRepository { get; }

    /// <summary>
    /// 权限存储库
    /// </summary>
    protected IPermissionDefinitionRecordRepository PermissionRepository { get; }

    /// <summary>
    /// 权限定义序列化器
    /// </summary>
    protected IPermissionDefinitionSerializer PermissionDefinitionSerializer { get; }

    /// <summary>
    /// 存储缓存
    /// </summary>
    protected IDynamicPermissionDefinitionStoreInMemoryCache StoreCache { get; }

    /// <summary>
    /// 分布式缓存
    /// </summary>
    protected IDistributedCache DistributedCache { get; }

    /// <summary>
    /// 分布式锁
    /// </summary>
    protected IAbpDistributedLock DistributedLock { get; }

    /// <summary>
    /// 权限管理配置
    /// </summary>
    public PermissionManagementOptions PermissionManagementOptions { get; }

    /// <summary>
    /// 分布式缓存配置
    /// </summary>
    protected AbpDistributedCacheOptions CacheOptions { get; }


    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="permissionGroupRepository"></param>
    /// <param name="permissionRepository"></param>
    /// <param name="permissionDefinitionSerializer"></param>
    /// <param name="storeCache"></param>
    /// <param name="distributedCache"></param>
    /// <param name="cacheOptions"></param>
    /// <param name="permissionManagementOptions"></param>
    /// <param name="distributedLock"></param>
    public DynamicPermissionDefinitionStore(
        IPermissionGroupDefinitionRecordRepository permissionGroupRepository,
        IPermissionDefinitionRecordRepository permissionRepository,
        IPermissionDefinitionSerializer permissionDefinitionSerializer,
        IDynamicPermissionDefinitionStoreInMemoryCache storeCache,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<PermissionManagementOptions> permissionManagementOptions,
        IAbpDistributedLock distributedLock)
    {
        PermissionGroupRepository = permissionGroupRepository;
        PermissionRepository = permissionRepository;
        PermissionDefinitionSerializer = permissionDefinitionSerializer;
        StoreCache = storeCache;
        DistributedCache = distributedCache;
        DistributedLock = distributedLock;
        PermissionManagementOptions = permissionManagementOptions.Value;
        CacheOptions = cacheOptions.Value;
    }

    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual async Task<PermissionDefinition?> GetOrNullAsync(string name)
    {
        if (!PermissionManagementOptions.IsDynamicPermissionStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetPermissionOrNull(name);
        }
    }

    /// <summary>
    /// 获取权限
    /// </summary>
    /// <returns></returns>
    public virtual async Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        if (!PermissionManagementOptions.IsDynamicPermissionStoreEnabled)
        {
            return Array.Empty<PermissionDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetPermissions().ToImmutableList();
        }
    }

    /// <summary>
    /// 获取权限组
    /// </summary>
    /// <returns></returns>
    public virtual async Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        if (!PermissionManagementOptions.IsDynamicPermissionStoreEnabled)
        {
            return Array.Empty<PermissionGroupDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetGroups().ToImmutableList();
        }
    }

    /// <summary>
    /// 确保缓存及时更新
    /// </summary>
    /// <returns></returns>
    protected virtual async Task EnsureCacheIsUptoDateAsync()
    {
        if (StoreCache.LastCheckTime.HasValue &&
            DateTime.Now.Subtract(StoreCache.LastCheckTime.Value).TotalSeconds < 30)
        {
            /* 为了优化，我们会稍微延迟获取最新的权限 */
            return;
        }

        var stampInDistributedCache = await GetOrSetStampInDistributedCache();

        if (stampInDistributedCache == StoreCache.CacheStamp)
        {
            StoreCache.LastCheckTime = DateTime.Now;
            return;
        }

        await UpdateInMemoryStoreCache();

        StoreCache.CacheStamp = stampInDistributedCache;
        StoreCache.LastCheckTime = DateTime.Now;
    }

    /// <summary>
    /// 更新内存存储缓存
    /// </summary>
    /// <returns></returns>
    protected virtual async Task UpdateInMemoryStoreCache()
    {
        var permissionGroupRecords = await PermissionGroupRepository.GetListAsync();
        var permissionRecords = await PermissionRepository.GetListAsync();

        await StoreCache.FillAsync(permissionGroupRecords, permissionRecords);
    }

    /// <summary>
    /// 获取或设置分布式缓存中的标记
    /// </summary>
    /// <returns></returns>
    /// <exception cref="AbpException"></exception>
    protected virtual async Task<string> GetOrSetStampInDistributedCache()
    {
        var cacheKey = GetCommonStampCacheKey();

        var stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache;
        }

        await using (var commonLockHandle = await DistributedLock
                         .TryAcquireAsync(GetCommonDistributedLockKey(), TimeSpan.FromMinutes(2)))
        {
            if (commonLockHandle == null)
            {
                /* This request will fail */
                throw new AbpException("无法获取用于权限定义公共标记检查的分布式锁!");
            }

            stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
            if (stampInDistributedCache != null)
            {
                return stampInDistributedCache;
            }

            stampInDistributedCache = Guid.NewGuid().ToString();

            await DistributedCache.SetStringAsync(
                cacheKey,
                stampInDistributedCache,
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
                }
            );
        }

        return stampInDistributedCache;
    }

    /// <summary>
    /// 获取通用戳记缓存密钥
    /// </summary>
    /// <returns></returns>
    protected virtual string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}:InMemory:PermissionCacheStamp";
    }

    /// <summary>
    /// 获取通用分布式锁密钥
    /// </summary>
    /// <returns></returns>
    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}:Permission:UpdateLock";
    }
}