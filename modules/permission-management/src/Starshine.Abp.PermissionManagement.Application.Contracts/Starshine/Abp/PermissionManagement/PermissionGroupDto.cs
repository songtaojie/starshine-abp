using System.Collections.Generic;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限组
/// </summary>
public class PermissionGroupDto
{
    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string DisplayName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? DisplayNameKey { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? DisplayNameResource { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<PermissionGrantInfoDto>? Permissions { get; set; }
}
