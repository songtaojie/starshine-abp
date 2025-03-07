using Starshine.Abp.Identity.Dtos;
using Starshine.Abp.Users;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace Starshine.Abp.Identity.Integration;

/// <summary>
/// 身份用户集成服务
/// </summary>
[IntegrationService]
public interface IIdentityUserIntegrationService : IApplicationService
{
    /// <summary>
    /// 根据主键获取角色名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<string[]> GetRoleNamesAsync(Guid id);

    /// <summary>
    /// 根据主键获取用户数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserData?> FindByIdAsync(Guid id);

    /// <summary>
    /// 根据用户名获取用户信息
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<UserData?> FindByUserNameAsync(string userName);

    /// <summary>
    /// 搜索用户信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input);

    /// <summary>
    /// 获取查询条数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(UserLookupCountInputDto input);
}