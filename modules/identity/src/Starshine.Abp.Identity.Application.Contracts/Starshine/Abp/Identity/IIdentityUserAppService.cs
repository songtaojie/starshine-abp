using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
public interface IIdentityUserAppService
    : ICrudAppService<
        IdentityUserDto,
        Guid,
        GetIdentityUsersInput,
        IdentityUserCreateDto,
        IdentityUserUpdateDto>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<IdentityUserDto?> FindByUsernameAsync(string userName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<IdentityUserDto?> FindByEmailAsync(string email);
}
