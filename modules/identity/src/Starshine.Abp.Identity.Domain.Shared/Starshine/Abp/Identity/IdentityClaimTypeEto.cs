using System;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份声明类型Eto
/// </summary>
[Serializable]
public class IdentityClaimTypeEto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 必需的
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// 是否静态
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    /// 正则表达式
    /// </summary>
    public string? Regex { get; set; }

    /// <summary>
    /// 正则表达式描述
    /// </summary>
    public string? RegexDescription { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 身份声明值类型
    /// </summary>
    public IdentityClaimValueType ValueType { get; set; }
}
