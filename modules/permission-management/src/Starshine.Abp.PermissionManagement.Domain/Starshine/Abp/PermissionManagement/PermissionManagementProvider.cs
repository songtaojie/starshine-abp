using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限管理提供者
/// </summary>
public abstract class PermissionManagementProvider : IPermissionManagementProvider
{
    /// <summary>
    /// 名称
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// 授权仓储
    /// </summary>
    protected IPermissionGrantRepository PermissionGrantRepository { get; }

    /// <summary>
    /// 主键生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }

    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionGrantRepository"></param>
    /// <param name="guidGenerator"></param>
    /// <param name="currentTenant"></param>
    protected PermissionManagementProvider(
        IPermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
    {
        PermissionGrantRepository = permissionGrantRepository;
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
    }

    /// <summary>
    /// 检查
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = await CheckAsync(new[] { name }, providerName, providerKey);

        return multiplePermissionValueProviderGrantInfo.Result.First().Value;
    }

    /// <summary>
    /// 检查
    /// </summary>
    /// <param name="names"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = new MultiplePermissionValueProviderGrantInfo(names);
        if (providerName != Name)
        {
            return multiplePermissionValueProviderGrantInfo;
        }

        var permissionGrants = await PermissionGrantRepository.GetListAsync(names, providerName, providerKey);

        foreach (var permissionName in names)
        {
            var isGrant = permissionGrants.Any(x => x.Name == permissionName);
            multiplePermissionValueProviderGrantInfo.Result[permissionName] = new PermissionValueProviderGrantInfo(isGrant, providerKey);
        }

        return multiplePermissionValueProviderGrantInfo;
    }

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerKey"></param>
    /// <param name="isGranted"></param>
    /// <returns></returns>
    public virtual Task SetAsync(string name, string providerKey, bool isGranted)
    {
        return isGranted
            ? GrantAsync(name, providerKey)
            : RevokeAsync(name, providerKey);
    }

    /// <summary>
    /// 授权
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected virtual async Task GrantAsync(string name, string providerKey)
    {
        var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
        if (permissionGrant != null)
        {
            return;
        }

        await PermissionGrantRepository.InsertAsync(
            new PermissionGrant(
                GuidGenerator.Create(),
                name,
                Name,
                providerKey,
                CurrentTenant.Id
            )
        );
    }

    /// <summary>
    /// 撤销
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected virtual async Task RevokeAsync(string name, string providerKey)
    {
        var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, providerKey);
        if (permissionGrant == null)
        {
            return;
        }

        await PermissionGrantRepository.DeleteAsync(permissionGrant);
    }
}
