using System.Collections.Generic;
using System.Threading.Tasks;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限查询
/// </summary>
public interface IPermissionFinder
{
    /// <summary>
    /// 是否授权
    /// </summary>
    /// <param name="requests">已授予请求</param>
    /// <returns></returns>
    Task<List<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> requests);
}
