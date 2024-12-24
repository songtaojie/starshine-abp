using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限管理器扩展方法
/// </summary>
public interface IPermissionManager
{
    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="permissionName"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<PermissionWithGrantedProviders> GetAsync(string permissionName, string providerName, string providerKey);

    /// <summary>
    /// 获取权限组
    /// </summary>
    /// <param name="permissionNames"></param>
    /// <param name="provideName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<MultiplePermissionWithGrantedProviders> GetAsync(string[] permissionNames, string provideName, string providerKey);

    /// <summary>
    /// 获取当前提供者所有权限
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<List<PermissionWithGrantedProviders>> GetAllAsync([NotNull] string providerName, [NotNull] string providerKey);

    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="permissionName"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="isGranted"></param>
    /// <returns></returns>
    Task SetAsync(string permissionName, string providerName, string providerKey, bool isGranted);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="permissionGrant"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task<PermissionGrant> UpdateProviderKeyAsync(PermissionGrant permissionGrant, string providerKey);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    Task DeleteAsync(string providerName, string providerKey);
}
