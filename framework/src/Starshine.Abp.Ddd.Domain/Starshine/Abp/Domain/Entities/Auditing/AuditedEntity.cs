using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// ��������ڼ�<see cref="IAuditedObject"/>��ʵ�֡�
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
/// ��������ڼ�<see cref="IAuditedObject"/>��ʵ�֡�
/// </summary>
/// <typeparam name="TKey">ʵ������������</typeparam>
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
