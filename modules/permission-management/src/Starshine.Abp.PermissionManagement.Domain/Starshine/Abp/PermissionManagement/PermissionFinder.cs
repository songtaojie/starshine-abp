using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// Ȩ�޲�����
/// </summary>
public class PermissionFinder : IPermissionFinder, ITransientDependency
{
    /// <summary>
    /// Ȩ��
    /// </summary>
    protected IPermissionManager PermissionManager { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionManager"></param>
    public PermissionFinder(IPermissionManager permissionManager)
    {
        PermissionManager = permissionManager;
    }

    /// <summary>
    /// �Ƿ���Ȩ
    /// </summary>
    /// <param name="requests"></param>
    /// <returns></returns>
    public virtual async Task<List<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> requests)
    {
        var result = new List<IsGrantedResponse>(requests.Count);
        foreach (var item in requests)
        {
            if (item.PermissionNames == null) continue;
            result.Add(new IsGrantedResponse
            {
                UserId = item.UserId,
                Permissions = (await PermissionManager.GetAsync(item.PermissionNames, UserPermissionValueProvider.ProviderName, item.UserId.ToString())).Result
                    .ToDictionary(x => x.Name, x => x.IsGranted)
            });
        }

        return result;
    }
}
