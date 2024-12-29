using System.Collections.Generic;
using JetBrains.Annotations;

namespace Starshine.Abp.Identity;

/// <summary>
/// 外部登录提供者词典
/// </summary>
public class ExternalLoginProviderDictionary : Dictionary<string, ExternalLoginProviderInfo>
{
    /// <summary>
    /// 添加或替换提供商。
    /// </summary>
    public void Add<TProvider>([NotNull] string name)where TProvider : IExternalLoginProvider
    {
        this[name] = new ExternalLoginProviderInfo(name, typeof(TProvider));
    }
}
