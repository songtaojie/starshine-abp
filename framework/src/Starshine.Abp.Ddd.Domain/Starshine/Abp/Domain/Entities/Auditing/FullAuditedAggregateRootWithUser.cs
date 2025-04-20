using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
///ʵ�� <see cref="IFullAuditedObject{TUser}"/> ��Ϊ��ȫ��ƾۺϸ��Ļ��ࡣ
/// </summary>
/// <typeparam name="TUser">�û�����</typeparam>
[Serializable]
public abstract class FullAuditedAggregateRootWithUser<TUser> : FullAuditedAggregateRoot, IFullAuditedObject<TUser>
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
/// ʵ�� <see cref="IFullAuditedObject{TUser}"/> ��Ϊ��ȫ��ƾۺϸ��Ļ��ࡣ
/// </summary>
/// <typeparam name="TKey">ʵ������������</typeparam>
/// <typeparam name="TUser">�û�����</typeparam>
[Serializable]
public abstract class FullAuditedAggregateRootWithUser<TKey, TUser> : FullAuditedAggregateRoot<TKey>, IFullAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    /// <inheritdoc />
    public virtual TUser? Deleter { get; set; }

    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    /// <inheritdoc />
    public virtual TUser? LastModifier { get; set; }

    protected FullAuditedAggregateRootWithUser()
    {

    }

    protected FullAuditedAggregateRootWithUser(TKey id)
        : base(id)
    {

    }
}
