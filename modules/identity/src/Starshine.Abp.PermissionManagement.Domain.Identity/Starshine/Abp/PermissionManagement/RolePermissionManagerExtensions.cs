using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Starshine.Abp.PermissionManagement;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;
/// <summary>
/// 角色权限管理器扩展
/// </summary>
public static class RolePermissionManagerExtensions
{
    /// <summary>
    /// 获取角色权限
    /// </summary>
    /// <param name="permissionManager"></param>
    /// <param name="roleName"></param>
    /// <param name="permissionName"></param>
    /// <returns></returns>
    public static Task<PermissionWithGrantedProviders> GetForRoleAsync([NotNull] this IPermissionManager permissionManager, string roleName, string permissionName)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.GetAsync(permissionName, RolePermissionValueProvider.ProviderName, roleName);
    }
    /// <summary>
    /// 获取角色所有权限
    /// </summary>
    /// <param name="permissionManager"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public static Task<List<PermissionWithGrantedProviders>> GetAllForRoleAsync([NotNull] this IPermissionManager permissionManager, string roleName)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.GetAllAsync(RolePermissionValueProvider.ProviderName, roleName);
    }

    /// <summary>
    /// 设置角色权限
    /// </summary>
    /// <param name="permissionManager"></param>
    /// <param name="roleName"></param>
    /// <param name="permissionName"></param>
    /// <param name="isGranted"></param>
    /// <returns></returns>
    public static Task SetForRoleAsync([NotNull] this IPermissionManager permissionManager, string roleName, [NotNull] string permissionName, bool isGranted)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.SetAsync(permissionName, RolePermissionValueProvider.ProviderName, roleName, isGranted);
    }
}
