using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// ��������ڼ�ʵ��<see cref="ICreationAuditedObject{TCreator}"/>��
/// </summary>
/// <typeparam name="TUser">�û�����</typeparam>
[Serializable]
public abstract class CreationAuditedEntityWithUser<TUser> : CreationAuditedEntity, ICreationAuditedObject<TUser>
{
    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }
}

/// <summary>
/// ��������ڼ�ʵ��<see cref="ICreationAuditedObject{TCreator}"/>��
/// </summary>
/// <typeparam name="TKey">ʵ������������</typeparam>
/// <typeparam name="TUser">�û�����</typeparam>
[Serializable]
public abstract class CreationAuditedEntityWithUser<TKey, TUser> : CreationAuditedEntity<TKey>, ICreationAuditedObject<TUser>
{
    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    protected CreationAuditedEntityWithUser()
    {

    }

    protected CreationAuditedEntityWithUser(TKey id)
        : base(id)
    {

    }
}
