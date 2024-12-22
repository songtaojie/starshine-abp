using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户查找服务接口
/// </summary>
/// <typeparam name="TUser"></typeparam>
public interface IUserLookupService<TUser>
    where TUser : class, IUser
{
    /// <summary>
    /// 根据id获取用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TUser?> FindByIdAsync( Guid id,CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据用户名获取用户信息
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TUser?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// 搜索用户信息
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="filter"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IUserData>> SearchAsync(string? sorting = null, string? filter = null,
        int maxResultCount = int.MaxValue,int skipCount = 0,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户条数
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(string? filter = null,CancellationToken cancellationToken = default);
}
