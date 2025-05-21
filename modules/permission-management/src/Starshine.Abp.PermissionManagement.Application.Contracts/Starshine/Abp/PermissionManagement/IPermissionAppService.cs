using JetBrains.Annotations;
using Starshine.Abp.Application.Services;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限应用服务
/// </summary>
public interface IPermissionAppService : IApplicationService
{
    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<GetPermissionListResultDto> GetAsync([NotNull] string providerName, [NotNull] string providerKey);

    /// <summary>
    /// 更新权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdatePermissionsDto input);
}
