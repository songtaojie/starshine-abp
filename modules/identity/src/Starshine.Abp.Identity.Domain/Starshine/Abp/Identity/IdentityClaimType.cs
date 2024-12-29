using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份声明类型
/// </summary>
public class IdentityClaimType : AggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; protected set; } = string.Empty;

    /// <summary>
    /// 是否必须
    /// </summary>
    public virtual bool Required { get; set; }

    /// <summary>
    /// 是否静态
    /// </summary>
    public virtual bool IsStatic { get; protected set; }

    /// <summary>
    /// 正则
    /// </summary>
    public virtual string? Regex { get; set; }

    /// <summary>
    /// 正则表达式描述
    /// </summary>
    public virtual string? RegexDescription { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 身份声明值类型
    /// </summary>
    public virtual IdentityClaimValueType ValueType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected IdentityClaimType()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="required"></param>
    /// <param name="isStatic"></param>
    /// <param name="regex"></param>
    /// <param name="regexDescription"></param>
    /// <param name="description"></param>
    /// <param name="valueType"></param>
    public IdentityClaimType(
        Guid id,
        [NotNull] string name,
        bool required = false,
        bool isStatic = false,
        [CanBeNull] string? regex = null,
        [CanBeNull] string? regexDescription = null,
        [CanBeNull] string? description = null,
        IdentityClaimValueType valueType = IdentityClaimValueType.String)
    {
        Id = id;
        SetName(name);
        Required = required;
        IsStatic = isStatic;
        Regex = regex;
        RegexDescription = regexDescription;
        Description = description;
        ValueType = valueType;
    }

    /// <summary>
    /// 设置名称
    /// </summary>
    /// <param name="name"></param>
    public void SetName([NotNull] string name)
    {
        Name = Check.NotNull(name, nameof(name));
    }
}
