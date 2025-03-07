using Starshine.Abp.Identity.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Starshine.Abp.Identity;
/// <summary>
/// 用户角色服务
/// </summary>
public interface IIdentityRoleAppService
    : ICrudAppService<
        IdentityRoleDto,
        Guid,
        GetIdentityRolesInputDto,
        IdentityRoleCreateDto,
        IdentityRoleUpdateDto>
{
    /// <summary>
    /// 获取所有角色列表
    /// </summary>
    /// <returns></returns>
    Task<ListResultDto<IdentityRoleDto>> GetAllListAsync();
}
