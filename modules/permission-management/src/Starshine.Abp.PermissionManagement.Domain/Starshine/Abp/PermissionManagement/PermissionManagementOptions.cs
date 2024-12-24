using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限配置
/// </summary>
public class PermissionManagementOptions
{
    /// <summary>
    /// 提供商
    /// </summary>
    public ITypeList<IPermissionManagementProvider> ManagementProviders { get; }

    /// <summary>
    /// 提供商策略
    /// </summary>
    public Dictionary<string, string> ProviderPolicies { get; }

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticPermissionsToDatabase { get; set; } = true;

    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicPermissionStoreEnabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public PermissionManagementOptions()
    {
        ManagementProviders = new TypeList<IPermissionManagementProvider>();
        ProviderPolicies = new Dictionary<string, string>();
    }
}
