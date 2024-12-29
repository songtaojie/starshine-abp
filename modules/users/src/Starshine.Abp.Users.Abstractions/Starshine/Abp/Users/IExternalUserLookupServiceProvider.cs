using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.Abp.Users;

/// <summary>
/// 外部用户查找服务提供者
/// </summary>
public interface IExternalUserLookupServiceProvider
{
    /// <summary>
    /// 根据用户id获取用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IUserData?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据用户名获取用户信息
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IUserData?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// 搜索用户信息
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="filter"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IUserData>> SearchAsync(string? sorting = null,string? filter = null,
        int maxResultCount = int.MaxValue,int skipCount = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// 用户用户总数
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(string? filter = null,CancellationToken cancellationToken = default);
}
