using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限管理
/// </summary>
public class PermissionManager : IPermissionManager, ISingletonDependency
{
    /// <summary>
    /// 授权仓储
    /// </summary>
    protected IPermissionGrantRepository PermissionGrantRepository { get; }

    /// <summary>
    /// 权限定义管理器
    /// </summary>
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    /// <summary>
    /// 状态检查
    /// </summary>
    protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }

    /// <summary>
    /// Guid生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }

    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }

    /// <summary>
    /// 
    /// </summary>
    protected IReadOnlyList<IPermissionManagementProvider> ManagementProviders => _lazyProviders.Value;

    /// <summary>
    /// 权限配置
    /// </summary>
    protected PermissionManagementOptions Options { get; }

    /// <summary>
    /// 分布式缓存
    /// </summary>
    protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

    private readonly Lazy<List<IPermissionManagementProvider>> _lazyProviders;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionDefinitionManager"></param>
    /// <param name="simpleStateCheckerManager"></param>
    /// <param name="permissionGrantRepository"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="guidGenerator"></param>
    /// <param name="options"></param>
    /// <param name="currentTenant"></param>
    /// <param name="cache"></param>
    public PermissionManager(
        IPermissionDefinitionManager permissionDefinitionManager,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager,
        IPermissionGrantRepository permissionGrantRepository,
        IServiceProvider serviceProvider,
        IGuidGenerator guidGenerator,
        IOptions<PermissionManagementOptions> options,
        ICurrentTenant currentTenant,
        IDistributedCache<PermissionGrantCacheItem> cache)
    {
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
        Cache = cache;
        SimpleStateCheckerManager = simpleStateCheckerManager;
        PermissionGrantRepository = permissionGrantRepository;
        PermissionDefinitionManager = permissionDefinitionManager;
        Options = options.Value;

        _lazyProviders = new Lazy<List<IPermissionManagementProvider>>(
            () => Options
                .ManagementProviders
                .Select(c => (serviceProvider.GetRequiredService(c) as IPermissionManagementProvider)!)
                .ToList(),
            true
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionName"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<PermissionWithGrantedProviders> GetAsync(string permissionName, string providerName, string providerKey)
    {
        var permission = await PermissionDefinitionManager.GetOrNullAsync(permissionName);
        if (permission == null)
        {
            return new PermissionWithGrantedProviders(permissionName, false);
        }

        return await GetInternalAsync(
            permission,
            providerName,
            providerKey
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionNames"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<MultiplePermissionWithGrantedProviders> GetAsync(string[] permissionNames, string providerName, string providerKey)
    {
        var permissions = new List<PermissionDefinition>();
        var undefinedPermissions = new List<string>();

        foreach (var permissionName in permissionNames)
        {
            var permission = await PermissionDefinitionManager.GetOrNullAsync(permissionName);
            if (permission != null)
            {
                permissions.Add(permission);
            }
            else
            {
                undefinedPermissions.Add(permissionName);
            }
        }

        if (permissions.Count == 0)
        {
            return new MultiplePermissionWithGrantedProviders(undefinedPermissions.ToArray());
        }

        var result = await GetInternalAsync(
            [.. permissions],
            providerName,
            providerKey
        );

        foreach (var undefinedPermission in undefinedPermissions)
        {
            result.Result.Add(new PermissionWithGrantedProviders(undefinedPermission, false));
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<List<PermissionWithGrantedProviders>> GetAllAsync(string providerName, string providerKey)
    {
        var permissionDefinitions = (await PermissionDefinitionManager.GetPermissionsAsync()).ToArray();

        var multiplePermissionWithGrantedProviders = await GetInternalAsync(permissionDefinitions, providerName, providerKey);

        return multiplePermissionWithGrantedProviders.Result;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionName"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="isGranted"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    /// <exception cref="AbpException"></exception>
    public virtual async Task SetAsync(string permissionName, string providerName, string providerKey, bool isGranted)
    {
        var permission = await PermissionDefinitionManager.GetOrNullAsync(permissionName);
        if (permission == null)
        {
            /* 默默忽略未定义的权限，也许它们已从动态权限定义存储中删除 */
            return;
        }

        if (!permission.IsEnabled || !await SimpleStateCheckerManager.IsEnabledAsync(permission))
        {
            throw new BusinessException($"权限[{permission.Name}]已被禁用!");
        }

        if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
        {
            throw new BusinessException($"提供程序[{providerName}] 未定义权限[{permission.Name}]");
        }

        if (!permission.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
        {
            throw new BusinessException($"权限[{permission.Name}]具有多租户端[{permission.MultiTenancySide}]，它与当前多租户端[{CurrentTenant.GetMultiTenancySide()}]不兼容");
        }

        var currentGrantInfo = await GetInternalAsync(permission, providerName, providerKey);
        if (currentGrantInfo.IsGranted == isGranted)
        {
            return;
        }

        var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName) ?? throw new BusinessException("未知的权限管理提供商：" + providerName);
        await provider.SetAsync(permissionName, providerKey, isGranted);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionGrant"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<PermissionGrant> UpdateProviderKeyAsync(PermissionGrant permissionGrant, string providerKey)
    {
        using (CurrentTenant.Change(permissionGrant.TenantId))
        {
            //使旧密钥的缓存无效
            await Cache.RemoveAsync(
                PermissionGrantCacheItem.CalculateCacheKey(
                    permissionGrant.Name,
                    permissionGrant.ProviderName,
                    permissionGrant.ProviderKey
                )
            );
        }

        permissionGrant.ProviderKey = providerKey;
        return await PermissionGrantRepository.UpdateAsync(permissionGrant);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(string providerName, string providerKey)
    {
        var permissionGrants = await PermissionGrantRepository.GetListAsync(providerName, providerKey);
        foreach (var permissionGrant in permissionGrants)
        {
            await PermissionGrantRepository.DeleteAsync(permissionGrant);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected virtual async Task<PermissionWithGrantedProviders> GetInternalAsync(
        PermissionDefinition permission,
        string providerName,
        string providerKey)
    {
        var multiplePermissionWithGrantedProviders = await GetInternalAsync(
            [permission],
            providerName,
            providerKey
        );

        return multiplePermissionWithGrantedProviders.Result.First();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissions"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected virtual async Task<MultiplePermissionWithGrantedProviders> GetInternalAsync(
        PermissionDefinition[] permissions,
        string providerName,
        string providerKey)
    {
        var permissionNames = permissions.Select(x => x.Name).ToArray();
        var multiplePermissionWithGrantedProviders = new MultiplePermissionWithGrantedProviders(permissionNames);

        var neededCheckPermissions = new List<PermissionDefinition>();

        foreach (var permission in permissions
                                    .Where(x => x.IsEnabled)
                                    .Where(x => x.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
                                    .Where(x => x.Providers.Count == 0 || x.Providers.Contains(providerName)))
        {
            if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
            {
                neededCheckPermissions.Add(permission);
            }
        }

        if (neededCheckPermissions.Count == 0)
        {
            return multiplePermissionWithGrantedProviders;
        }

        foreach (var provider in ManagementProviders)
        {
            permissionNames = neededCheckPermissions.Select(x => x.Name).ToArray();
            var multiplePermissionValueProviderGrantInfo = await provider.CheckAsync(permissionNames, providerName, providerKey);

            foreach (var providerResultDict in multiplePermissionValueProviderGrantInfo.Result)
            {
                if (providerResultDict.Value.IsGranted)
                {
                    var permissionWithGrantedProvider = multiplePermissionWithGrantedProviders.Result
                        .First(x => x.Name == providerResultDict.Key);

                    permissionWithGrantedProvider.IsGranted = true;
                    permissionWithGrantedProvider.Providers.Add(new PermissionValueProviderInfo(provider.Name, providerResultDict.Value.ProviderKey!));
                }
            }
        }

        return multiplePermissionWithGrantedProviders;
    }
}
