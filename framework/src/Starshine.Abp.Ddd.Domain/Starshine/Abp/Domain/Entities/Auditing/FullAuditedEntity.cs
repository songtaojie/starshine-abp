using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// ʵ�� <see cref="IFullAuditedObject"/> ��Ϊȫ�����ʵ��Ļ��ࡣ
/// </summary>
[Serializable]
public abstract class FullAuditedEntity : AuditedEntity, IFullAuditedObject
{
    /// <inheritdoc />
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    public virtual Guid? DeleterId { get; set; }

    /// <inheritdoc />
    public virtual DateTime? DeletionTime { get; set; }
}

/// <summary>
/// Implements <see cref="IFullAuditedObject"/> to be a base class for full-audited entities.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
[Serializable]
public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAuditedObject
{
    /// <inheritdoc />
    public virtual bool IsDeleted { get; set; }

    /// <inheritdoc />
    public virtual Guid? DeleterId { get; set; }

    /// <inheritdoc />
    public virtual DateTime? DeletionTime { get; set; }

    protected FullAuditedEntity()
    {

    }

    protected FullAuditedEntity(TKey id)
        : base(id)
    {

    }
}
