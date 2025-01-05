using Starshine.Abp.PermissionManagement;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement.Identity;
/// <summary>
/// 用户权限管理提供者
/// </summary>
public class UserPermissionManagementProvider : PermissionManagementProvider
{
    /// <summary>
    /// 提供者名称
    /// </summary>
    public override string Name => UserPermissionValueProvider.ProviderName;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionGrantRepository"></param>
    /// <param name="guidGenerator"></param>
    /// <param name="currentTenant"></param>
    public UserPermissionManagementProvider(
        IPermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    {

    }
}
