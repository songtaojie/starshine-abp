using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限管理提供商
/// </summary>
public interface IPermissionManagementProvider : ISingletonDependency //TODO: Consider to remove this pre-assumption
{
    /// <summary>
    /// 提供商名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 检查权限
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<PermissionValueProviderGrantInfo> CheckAsync([NotNull] string name, [NotNull] string providerName,[NotNull] string providerKey);

    /// <summary>
    /// 监察权限
    /// </summary>
    /// <param name="names"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<MultiplePermissionValueProviderGrantInfo> CheckAsync([NotNull] string[] names, [NotNull] string providerName,[NotNull] string providerKey);

    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerKey"></param>
    /// <param name="isGranted"></param>
    /// <returns></returns>
    Task SetAsync([NotNull] string name,[NotNull] string providerKey, bool isGranted);
}
