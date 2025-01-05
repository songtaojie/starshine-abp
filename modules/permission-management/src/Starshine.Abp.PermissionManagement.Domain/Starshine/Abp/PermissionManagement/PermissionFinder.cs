using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限查找器
/// </summary>
public class PermissionFinder : IPermissionFinder, ITransientDependency
{
    /// <summary>
    /// 权限管理器
    /// </summary>
    protected IPermissionManager PermissionManager { get; }

    /// <summary>
    /// 权限查找器
    /// </summary>
    /// <param name="permissionManager">权限管理器</param>
    public PermissionFinder(IPermissionManager permissionManager)
    {
        PermissionManager = permissionManager;
    }

    /// <summary>
    /// 是否授权
    /// </summary>
    /// <param name="requests">是否授予请求</param>
    /// <returns></returns>
    public virtual async Task<List<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> requests)
    {
        var result = new List<IsGrantedResponse>(requests.Count);
        foreach (var item in requests)
        {
            if (item.PermissionNames == null) continue;
            var permissionWithGrantedProviders = await PermissionManager.GetAsync(item.PermissionNames, UserPermissionValueProvider.ProviderName, item.UserId.ToString());
            result.Add(new IsGrantedResponse
            {
                UserId = item.UserId,
                Permissions = permissionWithGrantedProviders.Result.ToDictionary(x => x.Name, x => x.IsGranted)
            });
        }

        return result;
    }
}
