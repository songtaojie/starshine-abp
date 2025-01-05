using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
public interface IIdentityRoleAppService
    : ICrudAppService<
        IdentityRoleDto,
        Guid,
        GetIdentityRolesInput,
        IdentityRoleCreateDto,
        IdentityRoleUpdateDto>
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<ListResultDto<IdentityRoleDto>> GetAllListAsync();
}
