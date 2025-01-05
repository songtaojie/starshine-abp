using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Starshine.Abp.PermissionManagement;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

/// <summary>
/// 用户权限管理器扩展
/// </summary>
public static class UserPermissionManagerExtensions
{
    /// <summary>
    /// 获取所有权限
    /// </summary>
    /// <param name="permissionManager"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static Task<List<PermissionWithGrantedProviders>> GetAllForUserAsync([NotNull] this IPermissionManager permissionManager, Guid userId)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.GetAllAsync(UserPermissionValueProvider.ProviderName, userId.ToString());
    }
    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="permissionManager"></param>
    /// <param name="userId"></param>
    /// <param name="name"></param>
    /// <param name="isGranted"></param>
    /// <returns></returns>
    public static Task SetForUserAsync([NotNull] this IPermissionManager permissionManager, Guid userId, [NotNull] string name, bool isGranted)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.SetAsync(name, UserPermissionValueProvider.ProviderName, userId.ToString(), isGranted);
    }
}
