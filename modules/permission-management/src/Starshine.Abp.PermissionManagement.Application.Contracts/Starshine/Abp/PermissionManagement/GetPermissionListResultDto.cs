using System.Collections.Generic;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 
/// </summary>
public class GetPermissionListResultDto
{
    /// <summary>
    /// 显示名称
    /// </summary>
    public required string EntityDisplayName { get; set; }

    /// <summary>
    /// 组
    /// </summary>
    public required List<PermissionGroupDto> Groups { get; set; }
}
