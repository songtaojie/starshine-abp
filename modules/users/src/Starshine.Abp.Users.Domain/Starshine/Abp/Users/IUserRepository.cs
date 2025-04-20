using Starshine.Abp.Domain.Entities;
using Starshine.Abp.Domain.Repositories;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户仓储
/// </summary>
/// <typeparam name="TUser"></typeparam>
public interface IUserRepository<TUser> : IBasicRepository<TUser, Guid>
    where TUser : class, IUser, IAggregateRoot<Guid>
{
    /// <summary>
    /// 根据用户名获取用户信息
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TUser?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据id获取用户列表
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TUser>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// 搜索用户信息
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="filter"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TUser>> SearchAsync(string? sorting = null,string? filter = null,
        int maxResultCount = int.MaxValue,int skipCount = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户条数
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(string? filter = null,CancellationToken cancellationToken = default);
}
