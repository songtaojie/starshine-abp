using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Starshine.Abp.Domain.Entities;

/// <summary>
/// 聚合根基类。
/// </summary>
[Serializable]
public abstract class AggregateRoot : BasicAggregateRoot,
    IHasExtraProperties,
    IHasConcurrencyStamp
{
    /// <summary>
    /// 额外属性。
    /// </summary>
    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

    /// <summary>
    /// 版本控制戳。
    /// </summary>
    [DisableAuditing]
    public virtual string ConcurrencyStamp { get; set; }

    /// <summary>
    /// 构造函数。
    /// </summary>
    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = [];
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 验证。
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return ExtensibleObjectValidator.GetValidationErrors(
            this,
            validationContext
        );
    }
}

/// <summary>
/// 聚合根基类。
/// </summary>
/// <typeparam name="TKey"></typeparam>
[Serializable]
public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>,
    IHasExtraProperties,
    IHasConcurrencyStamp
{
    /// <summary>
    /// 额外属性。
    /// </summary>
    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

    /// <summary>
    /// 版本控制戳。
    /// </summary>
    [DisableAuditing]
    public virtual string ConcurrencyStamp { get; set; }

    /// <summary>
    /// 构造函数。
    /// </summary>
    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = [];
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 构造函数。
    /// </summary>
    /// <param name="id"></param>
    protected AggregateRoot(TKey id)
        : base(id)
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = [];
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 验证。
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return ExtensibleObjectValidator.GetValidationErrors(
            this,
            validationContext
        );
    }
}
