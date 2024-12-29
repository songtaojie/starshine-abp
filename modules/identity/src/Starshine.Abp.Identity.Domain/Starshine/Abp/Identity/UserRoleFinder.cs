using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Identity;

/// <summary>
/// 
/// </summary>
public class UserRoleFinder : IUserRoleFinder, ITransientDependency
{
    /// <summary>
    /// 
    /// </summary>
    protected IIdentityUserRepository IdentityUserRepository { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identityUserRepository"></param>
    public UserRoleFinder(IIdentityUserRepository identityUserRepository)
    {
        IdentityUserRepository = identityUserRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<string[]> GetRoleNamesAsync(Guid userId)
    {
        return (await IdentityUserRepository.GetRoleNamesAsync(userId)).ToArray();
    }
}
