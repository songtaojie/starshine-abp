using Starshine.Abp.Users;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Starshine.Abp.Identity.Integration;
/// <summary>
/// 身份用户集成服务
/// </summary>
public class IdentityUserIntegrationService : IdentityAppServiceBase, IIdentityUserIntegrationService
{
    /// <summary>
    /// 用户角色查找器
    /// </summary>
    protected IUserRoleFinder UserRoleFinder { get; }
    /// <summary>
    /// 
    /// </summary>
    protected ExternalUserLookupServiceProvider UserLookupServiceProvider { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userRoleFinder"></param>
    /// <param name="userLookupServiceProvider"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public IdentityUserIntegrationService(
        IUserRoleFinder userRoleFinder,
        ExternalUserLookupServiceProvider userLookupServiceProvider,
        IAbpLazyServiceProvider abpLazyServiceProvider):base(abpLazyServiceProvider)
    {
        UserRoleFinder = userRoleFinder;
        UserLookupServiceProvider = userLookupServiceProvider;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<string[]> GetRoleNamesAsync(Guid id)
    {
        return await UserRoleFinder.GetRoleNamesAsync(id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<UserData?> FindByIdAsync(Guid id)
    {
        var userData = await UserLookupServiceProvider.FindByIdAsync(id);
        if (userData == null) return null;
        return new UserData(userData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public virtual async Task<UserData?> FindByUserNameAsync(string userName)
    {
        var userData = await UserLookupServiceProvider.FindByUserNameAsync(userName);
        if (userData == null) return null;
        return new UserData(userData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
    {
        var users = await UserLookupServiceProvider.SearchAsync(input.Sorting,input.Filter,input.MaxResultCount,input.SkipCount);

        return new ListResultDto<UserData>(users.Select(u => new UserData(u)).ToList());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<long> GetCountAsync(UserLookupCountInputDto input)
    {
        return await UserLookupServiceProvider.GetCountAsync(input.Filter);
    }
}
