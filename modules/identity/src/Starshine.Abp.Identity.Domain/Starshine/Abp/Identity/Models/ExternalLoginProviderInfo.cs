using System;
using JetBrains.Annotations;
using Volo.Abp;

namespace Starshine.Abp.Identity;

/// <summary>
/// 外部登录提供者信息
/// </summary>
public class ExternalLoginProviderInfo
{
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 类型
    /// </summary>
    public Type Type
    {
        get => _type!;
        set => _type = Check.NotNull(value, nameof(value));
    }
    private Type? _type;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public ExternalLoginProviderInfo([NotNull] string name,[NotNull] Type type)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        Type = Check.AssignableTo<IExternalLoginProvider>(type, nameof(type));
    }
}
