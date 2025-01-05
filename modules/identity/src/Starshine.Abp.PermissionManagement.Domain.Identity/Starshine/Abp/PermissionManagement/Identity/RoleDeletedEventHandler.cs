using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Starshine.Abp.Identity;
using Volo.Abp.Uow;
using Starshine.Abp.PermissionManagement;

namespace Volo.Abp.PermissionManagement.Identity;

/// <summary>
/// 角色删除事件处理程序
/// </summary>
public class RoleDeletedEventHandler :IDistributedEventHandler<EntityDeletedEto<IdentityRoleEto>>,ITransientDependency
{
    /// <summary>
    /// 权限管理
    /// </summary>
    protected IPermissionManager PermissionManager { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionManager"></param>
    public RoleDeletedEventHandler(IPermissionManager permissionManager)
    {
        PermissionManager = permissionManager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEto<IdentityRoleEto> eventData)
    {
        await PermissionManager.DeleteAsync(RolePermissionValueProvider.ProviderName, eventData.Entity.Name);
    }
}
