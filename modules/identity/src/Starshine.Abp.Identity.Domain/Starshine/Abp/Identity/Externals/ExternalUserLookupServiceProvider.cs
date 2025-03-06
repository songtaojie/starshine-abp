using Microsoft.AspNetCore.Identity;
using Starshine.Abp.Users;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份用户存储库外部用户查找服务提供商
/// </summary>
public class ExternalUserLookupServiceProvider : IExternalUserLookupServiceProvider, ITransientDependency
{
    /// <summary>
    /// 用户存储库
    /// </summary>
    protected IIdentityUserRepository UserRepository { get; }
    /// <summary>
    /// 查找规范器
    /// </summary>
    protected ILookupNormalizer LookupNormalizer { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="lookupNormalizer"></param>
    public ExternalUserLookupServiceProvider(IIdentityUserRepository userRepository, ILookupNormalizer lookupNormalizer)
    {
        UserRepository = userRepository;
        LookupNormalizer = lookupNormalizer;
    }
    /// <summary>
    /// 根据id获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IUserData?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var identityUser = await UserRepository.FindAsync(id, includeDetails: false, cancellationToken: cancellationToken);
        if (identityUser == null) return null;
        return identityUser.ToStarshineUserData();
    }

    /// <summary>
    /// 根据名称获取
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IUserData?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        var identityUser = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName(userName), includeDetails: false, cancellationToken: cancellationToken);
        if (identityUser == null) return null;
        return identityUser.ToStarshineUserData();
    }

    /// <summary>
    /// 查找用户信息
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="filter"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IUserData>> SearchAsync(string? sorting = null,string? filter = null,int maxResultCount = int.MaxValue,int skipCount = 0,CancellationToken cancellationToken = default)
    {
        var users = await UserRepository.GetListAsync(
            sorting: sorting,
            maxResultCount: maxResultCount,
            skipCount: skipCount,
            filter: filter,
            includeDetails: false,
            cancellationToken: cancellationToken
        );

        return users.Select(u => u.ToStarshineUserData()).ToList();
    }

    /// <summary>
    /// 获取数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<long> GetCountAsync(string? filter = null, CancellationToken cancellationToken = new CancellationToken())
    {
        return await UserRepository.GetCountAsync(filter, cancellationToken: cancellationToken);
    }
}
