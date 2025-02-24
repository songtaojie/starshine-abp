using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.Users.EntityFrameworkCore;

/// <summary>
/// efcore用户仓储模块
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TUser"></typeparam>
public abstract class EfCoreUserRepositoryBase<TDbContext, TUser> : EfCoreRepository<TDbContext, TUser, Guid>, IUserRepository<TUser>
    where TDbContext : IEfCoreDbContext
    where TUser : class, IUser
{
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="dbContextProvider"></param>
    protected EfCoreUserRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    /// <summary>
    /// 根据用户名称获取用户数据
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TUser?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).OrderBy(x => x.Id).FirstOrDefaultAsync(u => u.UserName == userName, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<TUser>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 搜索用户数据
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<TUser>> SearchAsync(string? filter = null,string? sorting = null,
        int maxResultCount = int.MaxValue,int skipCount = 0, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!string.IsNullOrWhiteSpace(filter),
                u =>u.UserName.Contains(filter!) || u.Email.Contains(filter!) ||
                    (u.Name != null && u.Name.Contains(filter!)) || (u.Surname != null && u.Surname.Contains(filter!)))
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IUser.UserName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 获取用户条目
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<long> GetCountAsync(string? filter = null,CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>  u.UserName.Contains(filter!) || u.Email.Contains(filter!) ||
                    (u.Name != null && u.Name.Contains(filter!)) || (u.Surname != null && u.Surname.Contains(filter!))
            )
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
}
