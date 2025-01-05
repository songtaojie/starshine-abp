using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限定义序列化
/// </summary>
public interface IPermissionDefinitionSerializer
{
    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="permissionGroups">权限组定义</param>
    /// <returns>权限组定义记录，权限定义记录</returns>
    Task<(PermissionGroupDefinitionRecord[], PermissionDefinitionRecord[])>SerializeAsync(IEnumerable<PermissionGroupDefinition> permissionGroups);

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="permissionGroup">权限组定义</param>
    /// <returns>权限组定义记录</returns>
    Task<PermissionGroupDefinitionRecord> SerializeAsync(PermissionGroupDefinition permissionGroup);

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="permission">权限定义</param>
    /// <param name="permissionGroup">权限组定义</param>
    /// <returns>权限定义记录</returns>
    Task<PermissionDefinitionRecord> SerializeAsync( PermissionDefinition permission, [CanBeNull] PermissionGroupDefinition? permissionGroup);
}