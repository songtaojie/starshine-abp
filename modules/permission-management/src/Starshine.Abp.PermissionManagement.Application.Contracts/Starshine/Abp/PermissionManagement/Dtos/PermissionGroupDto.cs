using System.Collections.Generic;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限组
/// </summary>
public class PermissionGroupDto
{
    /// <summary>
    /// 权限组名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 权限组显示名称
    /// </summary>
    public required string DisplayName { get; set; }

    /// <summary>
    /// 权限组显示名称Key
    /// </summary>
    public string? DisplayNameKey { get; set; }

    /// <summary>
    /// 资源名称
    /// </summary>
    public string? DisplayNameResource { get; set; }

    /// <summary>
    /// 权限列表
    /// </summary>
    public List<PermissionGrantInfoDto>? Permissions { get; set; }
}
