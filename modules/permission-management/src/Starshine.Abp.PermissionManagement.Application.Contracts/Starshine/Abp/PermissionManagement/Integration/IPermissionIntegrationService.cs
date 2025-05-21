using Starshine.Abp.Application.Dtos;
using Starshine.Abp.Application.Services;
using Volo.Abp;

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
