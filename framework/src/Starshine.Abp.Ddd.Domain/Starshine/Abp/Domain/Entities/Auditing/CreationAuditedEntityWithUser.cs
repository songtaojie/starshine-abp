using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// 此类可用于简化实现<see cref="ICreationAuditedObject{TCreator}"/>。
/// </summary>
/// <typeparam name="TUser">用户类型</typeparam>
[Serializable]
public abstract class CreationAuditedEntityWithUser<TUser> : CreationAuditedEntity, ICreationAuditedObject<TUser>
{
    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }
}

/// <summary>
/// 此类可用于简化实现<see cref="ICreationAuditedObject{TCreator}"/>。
/// </summary>
/// <typeparam name="TKey">实体主键的类型</typeparam>
/// <typeparam name="TUser">用户类型</typeparam>
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
