using JetBrains.Annotations;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限提供者
/// </summary>
public class PermissionValueProviderGrantInfo //TODO: Rename to PermissionGrantInfo
{
    /// <summary>
    /// 
    /// </summary>
    public static PermissionValueProviderGrantInfo NonGranted { get; } = new PermissionValueProviderGrantInfo(false);

    /// <summary>
    /// 
    /// </summary>
    public virtual bool IsGranted { get; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string? ProviderKey { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isGranted"></param>
    /// <param name="providerKey"></param>
    public PermissionValueProviderGrantInfo(bool isGranted, string? providerKey = null)
    {
        IsGranted = isGranted;
        ProviderKey = providerKey;
    }
}
