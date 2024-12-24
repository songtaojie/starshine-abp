using JetBrains.Annotations;
using Volo.Abp;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 
/// </summary>
public class PermissionValueProviderInfo
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="key"></param>
    public PermissionValueProviderInfo([NotNull] string name, [NotNull] string key)
    {
        Check.NotNull(name, nameof(name));
        Check.NotNull(key, nameof(key));

        Name = name;
        Key = key;
    }
}
