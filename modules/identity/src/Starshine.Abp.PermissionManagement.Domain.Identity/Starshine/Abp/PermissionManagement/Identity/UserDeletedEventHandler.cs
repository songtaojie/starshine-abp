using Starshine.Abp.PermissionManagement;
using Starshine.Abp.Users;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.PermissionManagement.Identity;
/// <summary>
/// 用户删除事件处理程序
/// </summary>
public class UserDeletedEventHandler :IDistributedEventHandler<EntityDeletedEto<UserEto>>,ITransientDependency
{
    /// <summary>
    /// 
    /// </summary>
    protected IPermissionManager PermissionManager { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionManager"></param>
    public UserDeletedEventHandler(IPermissionManager permissionManager)
    {
        PermissionManager = permissionManager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEto<UserEto> eventData)
    {
        await PermissionManager.DeleteAsync(UserPermissionValueProvider.ProviderName, eventData.Entity.Id.ToString());
    }
}
