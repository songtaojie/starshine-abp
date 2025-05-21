using Starshine.Abp.Application.Dtos;
using Starshine.Abp.Application.Services;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.PermissionManagement.Integration;

/// <summary>
/// 权限集成服务
/// </summary>
[IntegrationService]
public class PermissionIntegrationService : ApplicationService, IPermissionIntegrationService
{
    /// <summary>
    /// 权限查找器
    /// </summary>
    protected IPermissionFinder PermissionFinder { get; }

    /// <summary>
    /// 权限集成服务
    /// </summary>
    /// <param name="permissionFinder">权限查找器</param>
    public PermissionIntegrationService(IPermissionFinder permissionFinder,IAbpLazyServiceProvider abpLazyServiceProvider):base(abpLazyServiceProvider)
    {
        PermissionFinder = permissionFinder;
    }

    /// <summary>
    /// 授予检查
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<ListResultDto<IsGrantedResponse>> IsGrantedAsync(List<IsGrantedRequest> input)
    {
        return new ListResultDto<IsGrantedResponse>(await PermissionFinder.IsGrantedAsync(input));
    }
}
