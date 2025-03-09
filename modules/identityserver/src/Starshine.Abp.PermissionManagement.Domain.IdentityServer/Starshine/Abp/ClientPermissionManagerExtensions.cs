using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Starshine.Abp.PermissionManagement;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;
/// <summary>
/// 客户端权限管理扩展方法
/// </summary>
public static class ClientPermissionManagerExtensions
{
    /// <summary>
    /// 获取客户端权限
    /// </summary>
    /// <param name="permissionManager"></param>
    /// <param name="clientId"></param>
    /// <param name="permissionName"></param>
    /// <returns></returns>
    public static Task<PermissionWithGrantedProviders> GetForClientAsync(this IPermissionManager permissionManager, string clientId, string permissionName)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.GetAsync(permissionName, ClientPermissionValueProvider.ProviderName, clientId);
    }

    /// <summary>
    /// 获取客户端所有权限
    /// </summary>
    /// <param name="permissionManager"></param>
    /// <param name="clientId"></param>
    /// <returns></returns>
    public static Task<List<PermissionWithGrantedProviders>> GetAllForClientAsync(this IPermissionManager permissionManager, string clientId)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.GetAllAsync(ClientPermissionValueProvider.ProviderName, clientId);
    }

    /// <summary>
    /// 设置客户端权限
    /// </summary>
    /// <param name="permissionManager"></param>
    /// <param name="clientId"></param>
    /// <param name="permissionName"></param>
    /// <param name="isGranted"></param>
    /// <returns></returns>
    public static Task SetForClientAsync(this IPermissionManager permissionManager, string clientId, [NotNull] string permissionName, bool isGranted)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.SetAsync(permissionName, ClientPermissionValueProvider.ProviderName, clientId, isGranted);
    }
}
