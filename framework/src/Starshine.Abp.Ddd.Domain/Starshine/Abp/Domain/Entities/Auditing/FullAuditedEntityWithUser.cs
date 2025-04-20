using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
///ʵ�� <see cref="IFullAuditedObject{TUser}"/> ��Ϊȫ�����ʵ��Ļ��ࡣ
/// </summary>
/// <typeparam name="TUser">Type of the user</typeparam>
[Serializable]
public abstract class FullAuditedEntityWithUser<TUser> : FullAuditedEntity, IFullAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    /// <inheritdoc />
    public virtual TUser? Deleter { get; set; }

    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    /// <inheritdoc />
    public virtual TUser? LastModifier { get; set; }
}

/// <summary>
/// ʵ�� <see cref="IFullAuditedObjectObject{TUser}"/> ��Ϊȫ�����ʵ��Ļ��ࡣ
/// </summary>
/// <typeparam name="TKey">ʵ������������</typeparam>
/// <typeparam name="TUser">�û�����</typeparam>
[Serializable]
public abstract class FullAuditedEntityWithUser<TKey, TUser> : FullAuditedEntity<TKey>, IFullAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    /// <inheritdoc />
    public virtual TUser? Deleter { get; set; }

    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    /// <inheritdoc />
    public virtual TUser? LastModifier { get; set; }

    protected FullAuditedEntityWithUser()
    {

    }

    protected FullAuditedEntityWithUser(TKey id)
        : base(id)
    {

    }
}
