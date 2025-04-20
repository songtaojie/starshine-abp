using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// ��������Ϳ����ڼ�ʵ��<see cref="IAuditedObject{TUser}"/>�ľۺϸ���ʵ�������
/// </summary>
/// <typeparam name="TUser">�û�����</typeparam>
[Serializable]
public abstract class AuditedAggregateRootWithUser<TUser> : AuditedAggregateRoot, IAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    /// <inheritdoc />
    public virtual TUser? LastModifier { get; set; }
}

/// <summary>
/// ��������Ϳ����ڼ�ʵ��<see cref="IAuditedObject{TUser}"/>�ľۺϸ���ʵ�������
/// </summary>
/// <typeparam name="TKey">ʵ������������</typeparam>
/// <typeparam name="TUser">�û�����</typeparam>
[Serializable]
public abstract class AuditedAggregateRootWithUser<TKey, TUser> : AuditedAggregateRoot<TKey>, IAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    /// <inheritdoc />
    public virtual TUser? LastModifier { get; set; }

    protected AuditedAggregateRootWithUser()
    {

    }

    protected AuditedAggregateRootWithUser(TKey id)
        : base(id)
    {

    }
}
