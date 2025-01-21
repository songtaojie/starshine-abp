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
public class PermissionIntegrationService : ApplicationService, IPermissionIntegrationService
{
    /// <summary>
    /// Ȩ�޲�����
    /// </summary>
    protected IPermissionFinder PermissionFinder { get; }

    /// <summary>
    /// Ȩ�޼��ɷ���
    /// </summary>
    /// <param name="permissionFinder">Ȩ�޲�����</param>
    public PermissionIntegrationService(IPermissionFinder permissionFinder)
    {
        PermissionFinder = permissionFinder;
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<ListResultDto<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> input)
    {
        return new ListResultDto<IsGrantedResponse>(await PermissionFinder.IsGrantedAsync(input));
    }
}
