using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// ��������ڼ򻯾ۺϸ��� <see cref="IAuditedObject"/> ʵ��.
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
///��������ڼ򻯾ۺϸ��� <see cref="IAuditedObject"/> ʵ�֡�
/// </summary>
/// <typeparam name="TKey">ʵ������������</typeparam>
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
