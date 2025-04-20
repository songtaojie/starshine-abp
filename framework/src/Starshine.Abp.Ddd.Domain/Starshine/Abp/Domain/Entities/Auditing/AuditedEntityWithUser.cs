using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
///此类可用于简化实现<see cref="IAuditedObject{TUser}"/>。
/// </summary>
/// <typeparam name="TUser">用户类型</typeparam>
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
///此类可用于简化实现<see cref="IAuditedObject{TUser}"/>。
/// </summary>
/// <typeparam name="TKey">实体主键的类型</typeparam>
/// <typeparam name="TUser">用户类型</typeparam>
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
