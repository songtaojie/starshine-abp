using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 
/// </summary>
public class PermissionWithGrantedProviders
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsGranted { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<PermissionValueProviderInfo> Providers { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isGranted"></param>
    public PermissionWithGrantedProviders([NotNull] string name, bool isGranted)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
        IsGranted = isGranted;

        Providers = new List<PermissionValueProviderInfo>();
    }
}
