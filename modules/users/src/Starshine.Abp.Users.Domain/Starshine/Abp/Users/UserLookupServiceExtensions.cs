using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户搜索扩展类
/// </summary>
public static class UserLookupServiceExtensions
{
    /// <summary>
    /// 根据主键获取用户信息
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="userLookupService"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public static async Task<TUser> GetByIdAsync<TUser>(this IUserLookupService<TUser> userLookupService, Guid id, CancellationToken cancellationToken = default)
        where TUser : class, IUser
    {
        var user = await userLookupService.FindByIdAsync(id, cancellationToken) ?? throw new EntityNotFoundException(typeof(TUser), id);
        return user;
    }

    /// <summary>
    /// 根据用户名获取用户信息
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="userLookupService"></param>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public static async Task<TUser> GetByUserNameAsync<TUser>(this IUserLookupService<TUser> userLookupService, string userName, CancellationToken cancellationToken = default)
        where TUser : class, IUser
    {
        var user = await userLookupService.FindByUserNameAsync(userName, cancellationToken) ?? throw new EntityNotFoundException(typeof(TUser), userName);
        return user;
    }
}
