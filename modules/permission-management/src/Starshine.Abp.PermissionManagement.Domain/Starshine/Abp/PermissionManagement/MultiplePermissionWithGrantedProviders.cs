using System.Collections.Generic;
using Volo.Abp;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 多权限带提供者
/// </summary>
public class MultiplePermissionWithGrantedProviders
{
    /// <summary>
    /// 结果集
    /// </summary>
    public List<PermissionWithGrantedProviders> Result { get; }

    /// <summary>
    /// 
    /// </summary>
    public MultiplePermissionWithGrantedProviders()
    {
        Result = [];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="names"></param>
    public MultiplePermissionWithGrantedProviders(string[] names)
    {
        Check.NotNull(names, nameof(names));

        Result = new List<PermissionWithGrantedProviders>(names.Length);

        foreach (var name in names)
        {
            Result.Add(new PermissionWithGrantedProviders(name, false));
        }
    }
}
