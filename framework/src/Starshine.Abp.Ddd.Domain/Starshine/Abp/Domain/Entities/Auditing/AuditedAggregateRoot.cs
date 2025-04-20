using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// 此类可用于简化聚合根的 <see cref="IAuditedObject"/> 实现.
/// </summary>
[Serializable]
public abstract class AuditedAggregateRoot : CreationAuditedAggregateRoot, IAuditedObject
{
    /// <inheritdoc />
    public virtual DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }
}

/// <summary>
///此类可用于简化聚合根的 <see cref="IAuditedObject"/> 实现。
/// </summary>
/// <typeparam name="TKey">实体主键的类型</typeparam>
[Serializable]
public abstract class AuditedAggregateRoot<TKey> : CreationAuditedAggregateRoot<TKey>, IAuditedObject
{
    /// <inheritdoc />
    public virtual DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }

    protected AuditedAggregateRoot()
    {

    }

    protected AuditedAggregateRoot(TKey id)
        : base(id)
    {

    }
}
