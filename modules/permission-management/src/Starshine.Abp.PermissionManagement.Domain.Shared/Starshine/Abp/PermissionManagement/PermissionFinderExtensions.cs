using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限查找器扩展
/// </summary>
public static class PermissionFinderExtensions
{
    /// <summary>
    /// 是否已授权
    /// </summary>
    /// <param name="permissionFinder">权限查找器</param>
    /// <param name="userId">用户id</param>
    /// <param name="permissionName">权限名称</param>
    /// <returns></returns>
    public async static Task<bool> IsGrantedAsync(this IPermissionFinder permissionFinder, Guid userId, string permissionName)
    {
        return await permissionFinder.IsGrantedAsync(userId, new[] { permissionName });
    }

    /// <summary>
    /// 是否已授权
    /// </summary>
    /// <param name="permissionFinder">权限查找器</param>
    /// <param name="userId">用户id</param>
    /// <param name="permissionNames">权限名称</param>
    /// <returns></returns>
    public async static Task<bool> IsGrantedAsync(this IPermissionFinder permissionFinder, Guid userId, string[] permissionNames)
    {
        return (await permissionFinder.IsGrantedAsync(
        [
            new IsGrantedRequest
            {
                UserId = userId,
                PermissionNames = permissionNames
            }
        ])).Any(x => x.UserId == userId && x.Permissions.All(p => permissionNames.Contains(p.Key) && p.Value));
    }
}
