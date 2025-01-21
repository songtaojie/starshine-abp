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
    /// 将静态权限保存到数据库
    /// Default: true.
    /// </summary>
    public bool SaveStaticPermissionsToDatabase { get; set; } = true;

    /// <summary>
    /// 是否启用动态权限存储
    /// Default: false.
    /// </summary>
    public bool IsDynamicPermissionStoreEnabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public PermissionManagementOptions()
    {
        ManagementProviders = new TypeList<IPermissionManagementProvider>();
        ProviderPolicies = [];
    }
}
