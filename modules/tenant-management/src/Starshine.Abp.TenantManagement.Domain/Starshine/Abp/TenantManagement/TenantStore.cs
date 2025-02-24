using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.TenantManagement;

/// <summary>
/// 租户存储
/// </summary>
public class TenantStore : ITenantStore, ITransientDependency
{
    /// <summary>
    /// 租户存储
    /// </summary>
    protected ITenantRepository TenantRepository { get; }

    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }

    /// <summary>
    /// 分布式缓存
    /// </summary>
    protected IDistributedCache<TenantConfigurationCacheItem> Cache { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tenantRepository"></param>
    /// <param name="currentTenant"></param>
    /// <param name="cache"></param>
    public TenantStore(
        ITenantRepository tenantRepository,
        ICurrentTenant currentTenant,
        IDistributedCache<TenantConfigurationCacheItem> cache)
    {
        TenantRepository = tenantRepository;
        CurrentTenant = currentTenant;
        Cache = cache;
    }

    /// <summary>
    /// 根据租户名称查找租户配置
    /// </summary>
    /// <param name="normalizedName"></param>
    /// <returns></returns>
    public virtual async Task<TenantConfiguration?> FindAsync(string normalizedName)
    {
        return (await GetCacheItemAsync(null, normalizedName)).Value;
    }

    /// <summary>
    /// 根据租户ID查找租户配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<TenantConfiguration?> FindAsync(Guid id)
    {
        return (await GetCacheItemAsync(id, null)).Value;
    }

    /// <summary>
    /// 获取租户列表
    /// </summary>
    /// <param name="includeDetails"></param>
    /// <returns></returns>
    public virtual async Task<IReadOnlyList<TenantConfiguration>> GetListAsync(bool includeDetails = false)
    {
        var tenantList = await TenantRepository.GetListAsync(includeDetails);
        return tenantList.ConvertAll(t => new TenantConfiguration
        { 
            Id = t.Id,
            Name = t.Name,  
            NormalizedName = t.NormalizedName,
            ConnectionStrings = GetConnectionStrings(t),
            IsActive = false,
        });
    }

    /// <summary>
    /// 根据租户名称查找租户配置
    /// </summary>
    /// <param name="normalizedName"></param>
    /// <returns></returns>
    [Obsolete("Use FindAsync method.")]
    public virtual TenantConfiguration? Find(string normalizedName)
    {
        return FindAsync(normalizedName).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 根据租户ID查找租户配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Obsolete("Use FindAsync method.")]
    public virtual TenantConfiguration? Find(Guid id)
    {
        return FindAsync(id).GetAwaiter().GetResult();
    }

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <param name="id"></param>
    /// <param name="normalizedName"></param>
    /// <returns></returns>
    /// <exception cref="AbpException"></exception>
    protected virtual async Task<TenantConfigurationCacheItem> GetCacheItemAsync(Guid? id, string? normalizedName)
    {
        var cacheKey = CalculateCacheKey(id, normalizedName);

        var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);
        if (cacheItem?.Value != null)
        {
            return cacheItem;
        }

        if (id.HasValue)
        {
            using (CurrentTenant.Change(null)) //TODO: 如果我们可以实现定义主机端（或独立于租户的）实体，就不需要这样做！
            {
                var tenant = await TenantRepository.FindAsync(id.Value);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        if (!normalizedName.IsNullOrWhiteSpace())
        {
            using (CurrentTenant.Change(null)) //TODO: 如果我们可以实现定义主机端（或独立于租户的）实体，就不需要这样做！
            {
                var tenant = await TenantRepository.FindByNameAsync(normalizedName);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        throw new AbpException("Both id and normalizedName can't be invalid.");
    }

    /// <summary>
    /// 设置缓存
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="tenant"></param>
    /// <returns></returns>
    protected virtual async Task<TenantConfigurationCacheItem> SetCacheAsync(string cacheKey, Tenant? tenant)
    {
        var tenantConfiguration = tenant != null ? new TenantConfiguration
        { 
            Id = tenant.Id,
        }: null;
        var cacheItem = new TenantConfigurationCacheItem(tenantConfiguration);
        await Cache.SetAsync(cacheKey, cacheItem, considerUow: true);
        return cacheItem;
    }
    
    private static ConnectionStrings GetConnectionStrings(Tenant tenant)
    {
        var connStrings = new ConnectionStrings();

        if (tenant.ConnectionStrings == null)
        {
            return connStrings;
        }

        foreach (var connectionString in tenant.ConnectionStrings)
        {
            connStrings[connectionString.Name] = connectionString.Value;
        }

        return connStrings;
    }

    /// <summary>
    /// 计算缓存键
    /// </summary>
    /// <param name="id"></param>
    /// <param name="normalizedName"></param>
    /// <returns></returns>
    protected virtual string CalculateCacheKey(Guid? id, string? normalizedName)
    {
        return TenantConfigurationCacheItem.CalculateCacheKey(id, normalizedName);
    }
}
