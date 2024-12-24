using System;
using System.Linq;
using Volo.Abp.Text.Formatting;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 授权缓存
/// </summary>
[Serializable]
public class PermissionGrantCacheItem
{
    private const string CacheKeyFormat = "pn:{0},pk:{1},n:{2}";

    /// <summary>
    /// 收否授权
    /// </summary>
    public bool IsGranted { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public PermissionGrantCacheItem()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isGranted"></param>
    public PermissionGrantCacheItem(bool isGranted)
    {
        IsGranted = isGranted;
    }

    /// <summary>
    /// 计算缓存key
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public static string CalculateCacheKey(string name, string providerName, string? providerKey)
    {
        return string.Format(CacheKeyFormat, providerName, providerKey, name);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <returns></returns>
    public static string? GetPermissionNameFormCacheKeyOrNull(string cacheKey)
    {
        var result = FormattedStringValueExtracter.Extract(cacheKey, CacheKeyFormat, true);
        return result.IsMatch ? result.Matches.Last().Value : null;
    }
}
