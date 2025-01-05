using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Starshine.Abp.Identity;
using Starshine.Abp.PermissionManagement;

namespace Volo.Abp.PermissionManagement.Identity;
/// <summary>
/// 角色更新事件处理程序
/// </summary>
public class RoleUpdateEventHandler :IDistributedEventHandler<IdentityRoleNameChangedEto>, ITransientDependency
{
    /// <summary>
    /// 权限管理器
    /// </summary>
    protected IPermissionManager PermissionManager { get; }

    /// <summary>
    /// 权限授予存储库
    /// </summary>
    protected IPermissionGrantRepository PermissionGrantRepository { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionManager"></param>
    /// <param name="permissionGrantRepository"></param>
    public RoleUpdateEventHandler(
        IPermissionManager permissionManager,
        IPermissionGrantRepository permissionGrantRepository)
    {
        PermissionManager = permissionManager;
        PermissionGrantRepository = permissionGrantRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public async Task HandleEventAsync(IdentityRoleNameChangedEto eventData)
    {
        var permissionGrantsInRole = await PermissionGrantRepository.GetListAsync(RolePermissionValueProvider.ProviderName, eventData.OldName);
        foreach (var permissionGrant in permissionGrantsInRole)
        {
            await PermissionManager.UpdateProviderKeyAsync(permissionGrant, eventData.Name);
        }
    }
}
