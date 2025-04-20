using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
///��������ڼ�ʵ��<see cref="IAuditedObject{TUser}"/>��
/// </summary>
/// <typeparam name="TUser">�û�����</typeparam>
[Serializable]
public abstract class AuditedEntityWithUser<TUser> : AuditedEntity, IAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    /// <inheritdoc />
    public virtual TUser? LastModifier { get; set; }
}

/// <summary>
///��������ڼ�ʵ��<see cref="IAuditedObject{TUser}"/>��
/// </summary>
/// <typeparam name="TKey">ʵ������������</typeparam>
/// <typeparam name="TUser">�û�����</typeparam>
[Serializable]
public abstract class AuditedEntityWithUser<TKey, TUser> : AuditedEntity<TKey>, IAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    /// <inheritdoc />
    public virtual TUser? LastModifier { get; set; }

    protected AuditedEntityWithUser()
    {

    }

    protected AuditedEntityWithUser(TKey id)
        : base(id)
    {

    }
}
