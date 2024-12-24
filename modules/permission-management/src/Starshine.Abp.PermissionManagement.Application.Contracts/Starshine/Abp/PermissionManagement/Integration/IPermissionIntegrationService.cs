using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Starshine.Abp.PermissionManagement.Integration;

/// <summary>
/// Ȩ�޼��ɷ���
/// </summary>
[IntegrationService]
public interface IPermissionIntegrationService : IApplicationService
{
    /// <summary>
    /// �Ƿ���Ȩ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ListResultDto<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> input);
}
