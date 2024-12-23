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
    /// 
    /// </summary>
    /// <param name="permissionGroups"></param>
    /// <returns></returns>
    Task<(PermissionGroupDefinitionRecord[], PermissionDefinitionRecord[])>SerializeAsync(IEnumerable<PermissionGroupDefinition> permissionGroups);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionGroup"></param>
    /// <returns></returns>
    Task<PermissionGroupDefinitionRecord> SerializeAsync(PermissionGroupDefinition permissionGroup);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permission"></param>
    /// <param name="permissionGroup"></param>
    /// <returns></returns>
    Task<PermissionDefinitionRecord> SerializeAsync( PermissionDefinition permission, [CanBeNull] PermissionGroupDefinition permissionGroup);
}