using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// 此类可用于简化聚合根的 <see cref="ICreationAuditedObject"/> 的实现。
/// </summary>
[Serializable]
public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
{
    /// <inheritdoc />
    public virtual DateTime CreationTime { get; protected set; }

    /// <inheritdoc />
    public virtual Guid? CreatorId { get; protected set; }
}

/// <summary>
/// 此类可用于简化聚合根的 <see cref="ICreationAuditedObject"/> 的实现。
/// </summary>
/// <typeparam name="TKey">实体主键的类型</typeparam>
[Serializable]
public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedObject
{
    /// <inheritdoc />
    public virtual DateTime CreationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? CreatorId { get; set; }

    protected CreationAuditedAggregateRoot()
    {

    }

    protected CreationAuditedAggregateRoot(TKey id)
        : base(id)
    {

    }
}
