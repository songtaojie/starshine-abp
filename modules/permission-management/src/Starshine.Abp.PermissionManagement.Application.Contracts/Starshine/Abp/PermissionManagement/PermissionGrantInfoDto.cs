using System.Collections.Generic;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限信息dto
/// </summary>
public class PermissionGrantInfoDto
{
    /// <summary>
    /// 权限名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 父级名称
    /// </summary>
    public string? ParentName { get; set; }

    /// <summary>
    /// 是否授权
    /// </summary>
    public bool IsGranted { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required List<string> AllowedProviders { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<ProviderInfoDto>? GrantedProviders { get; set; }
}
