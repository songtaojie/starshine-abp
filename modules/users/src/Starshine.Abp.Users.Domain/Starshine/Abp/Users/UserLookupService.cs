using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户查询抽象类
/// </summary>
/// <typeparam name="TUser"></typeparam>
/// <typeparam name="TUserRepository"></typeparam>
public abstract class UserLookupService<TUser, TUserRepository> : IUserLookupService<TUser>, ITransientDependency
    where TUser : class, IUser
    where TUserRepository : IUserRepository<TUser>
{
    /// <summary>
    /// 如果本地用户存在则跳过外部查找
    /// </summary>
    protected bool SkipExternalLookupIfLocalUserExists { get; set; } = true;

    /// <summary>
    /// 外部查询
    /// </summary>
    protected readonly IExternalUserLookupServiceProvider ExternalUserLookupServiceProvider;

    /// <summary>
    /// 日志对象
    /// </summary>
    protected readonly ILogger<UserLookupService<TUser, TUserRepository>> Logger;

    private readonly TUserRepository _userRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="unitOfWorkManager"></param>
    /// <param name="externalUserLookupServiceProvider"></param>
    protected UserLookupService(TUserRepository userRepository,IUnitOfWorkManager unitOfWorkManager,
        IExternalUserLookupServiceProvider externalUserLookupServiceProvider)
    {
        _userRepository = userRepository;
        _unitOfWorkManager = unitOfWorkManager;
        ExternalUserLookupServiceProvider = externalUserLookupServiceProvider;
        Logger = NullLogger<UserLookupService<TUser, TUserRepository>>.Instance;
    }

    /// <summary>
    /// 根据主键获取用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TUser?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var localUser = await _userRepository.FindAsync(id, cancellationToken: cancellationToken);

        if (ExternalUserLookupServiceProvider == null)
        {
            return localUser;
        }

        if (SkipExternalLookupIfLocalUserExists && localUser != null)
        {
            return localUser;
        }

        IUserData externalUser;

        try
        {
            externalUser = await ExternalUserLookupServiceProvider.FindByIdAsync(id, cancellationToken);
            if (externalUser == null)
            {
                if (localUser != null)
                {
                    //TODO: 不应该删除，而应该使其处于非活动状态或诸如此类？
                    await WithNewUowAsync(() => _userRepository.DeleteAsync(localUser, cancellationToken: cancellationToken));
                }

                return null;
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            return localUser;
        }

        if (localUser == null)
        {
            await WithNewUowAsync(() => _userRepository.InsertAsync(CreateUser(externalUser), cancellationToken: cancellationToken));
            return await _userRepository.FindAsync(id, cancellationToken: cancellationToken);
        }

        if (localUser is IUpdateUserData && ((IUpdateUserData)localUser).Update(externalUser))
        {
            await WithNewUowAsync(() => _userRepository.UpdateAsync(localUser, cancellationToken: cancellationToken));
        }
        else
        {
            return localUser;
        }

        return await _userRepository.FindAsync(id, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 根据用户名获取用户信息
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TUser?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        var localUser = await _userRepository.FindByUserNameAsync(userName, cancellationToken);

        if (ExternalUserLookupServiceProvider == null)
        {
            return localUser;
        }

        if (SkipExternalLookupIfLocalUserExists && localUser != null)
        {
            return localUser;
        }

        IUserData externalUser;

        try
        {
            externalUser = await ExternalUserLookupServiceProvider.FindByUserNameAsync(userName, cancellationToken);
            if (externalUser == null)
            {
                if (localUser != null)
                {
                    //TODO: 不应该删除，而应该使其成为被动的或诸如此类的？
                    await WithNewUowAsync(() => _userRepository.DeleteAsync(localUser, cancellationToken: cancellationToken));
                }

                return null;
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            return localUser;
        }

        if (localUser == null)
        {
            await WithNewUowAsync(() => _userRepository.InsertAsync(CreateUser(externalUser), cancellationToken: cancellationToken));
            return await _userRepository.FindAsync(externalUser.Id, cancellationToken: cancellationToken);
        }

        if (localUser is IUpdateUserData && ((IUpdateUserData)localUser).Update(externalUser))
        {
            await WithNewUowAsync(() => _userRepository.UpdateAsync(localUser, cancellationToken: cancellationToken));
        }
        else
        {
            return localUser;
        }

        return await _userRepository.FindAsync(externalUser.Id, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 搜索用户信息
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="filter"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<IUserData>> SearchAsync(string? sorting = null,string? filter = null,
        int maxResultCount = int.MaxValue,int skipCount = 0, CancellationToken cancellationToken = default)
    {
        if (ExternalUserLookupServiceProvider != null)
        {
            return await ExternalUserLookupServiceProvider.SearchAsync(sorting,filter,maxResultCount,skipCount,cancellationToken);
        }

        var localUsers = await _userRepository.SearchAsync(sorting,filter, maxResultCount,skipCount,cancellationToken);

        return localUsers.Cast<IUserData>().ToList();
    }

    /// <summary>
    /// 获取用户条目
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<long> GetCountAsync(string? filter = null, CancellationToken cancellationToken = default)
    {
        if (ExternalUserLookupServiceProvider != null)
        {
            return await ExternalUserLookupServiceProvider.GetCountAsync(filter,cancellationToken);
        }

        return await _userRepository .GetCountAsync( filter,cancellationToken);
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="externalUser"></param>
    /// <returns></returns>
    protected abstract TUser CreateUser(IUserData externalUser);

    private async Task WithNewUowAsync(Func<Task> func)
    {
        using var uow = _unitOfWorkManager.Begin(requiresNew: true);
        await func();
        await uow.CompleteAsync();
    }
}
