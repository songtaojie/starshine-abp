using Starshine.Abp.PermissionManagement;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.PermissionManagement.IdentityServer;
/// <summary>
/// 客户端权限管理提供者
/// </summary>
public class ClientPermissionManagementProvider : PermissionManagementProvider
{
    /// <summary>
    /// 客户端权限管理提供者
    /// </summary>
    public override string Name => ClientPermissionValueProvider.ProviderName;

    /// <summary>
    /// 客户端权限管理提供者
    /// </summary>
    /// <param name="permissionGrantRepository"></param>
    /// <param name="guidGenerator"></param>
    /// <param name="currentTenant"></param>
    public ClientPermissionManagementProvider(
        IPermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    {

    }

    /// <summary>
    /// 检查权限
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public override Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
    {
        using (CurrentTenant.Change(null))
        {
            return base.CheckAsync(name, providerName, providerKey);
        }
    }

    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected override Task GrantAsync(string name, string providerKey)
    {
        using (CurrentTenant.Change(null))
        {
            return base.GrantAsync(name, providerKey);
        }
    }

    /// <summary>
    /// 撤销权限
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    protected override Task RevokeAsync(string name, string providerKey)
    {
        using (CurrentTenant.Change(null))
        {
            return base.RevokeAsync(name, providerKey);
        }
    }

    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerKey"></param>
    /// <param name="isGranted"></param>
    /// <returns></returns>
    public override Task SetAsync(string name, string providerKey, bool isGranted)
    {
        using (CurrentTenant.Change(null))
        {
            return base.SetAsync(name, providerKey, isGranted);
        }
    }
}
