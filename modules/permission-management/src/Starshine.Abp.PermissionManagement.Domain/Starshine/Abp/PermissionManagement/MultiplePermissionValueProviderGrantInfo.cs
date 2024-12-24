using System.Collections.Generic;
using Volo.Abp;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 多个权限
/// </summary>
public class MultiplePermissionValueProviderGrantInfo
{
    /// <summary>
    /// 结果集
    /// </summary>
    public Dictionary<string, PermissionValueProviderGrantInfo> Result { get; }

    /// <summary>
    /// 
    /// </summary>
    public MultiplePermissionValueProviderGrantInfo()
    {
        Result = [];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="names"></param>
    public MultiplePermissionValueProviderGrantInfo(string[] names)
    {
        Check.NotNull(names, nameof(names));
        Result = new Dictionary<string, PermissionValueProviderGrantInfo>(names.Length);
        foreach (var name in names)
        {
            Result.Add(name, PermissionValueProviderGrantInfo.NonGranted);
        }
    }
}
