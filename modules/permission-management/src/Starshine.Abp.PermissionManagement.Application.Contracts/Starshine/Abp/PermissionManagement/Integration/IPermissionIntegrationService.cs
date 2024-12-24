using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Starshine.Abp.PermissionManagement.Integration;

/// <summary>
/// 权限集成服务
/// </summary>
[IntegrationService]
public interface IPermissionIntegrationService : IApplicationService
{
    /// <summary>
    /// 是否授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ListResultDto<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> input);
}
