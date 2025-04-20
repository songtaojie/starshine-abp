using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// 此类可用于简化<see cref="IAuditedObject"/>的实现。
/// </summary>
[Serializable]
public abstract class AuditedEntity : CreationAuditedEntity, IAuditedObject
{
    /// <inheritdoc />
    public virtual DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }
}

/// <summary>
/// 此类可用于简化<see cref="IAuditedObject"/>的实现。
/// </summary>
/// <typeparam name="TKey">实体主键的类型</typeparam>
[Serializable]
public abstract class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IAuditedObject
{
    /// <inheritdoc />
    public virtual DateTime? LastModificationTime { get; set; }

    /// <inheritdoc />
    public virtual Guid? LastModifierId { get; set; }

    protected AuditedEntity()
    {

    }

    protected AuditedEntity(TKey id)
        : base(id)
    {

    }
}
