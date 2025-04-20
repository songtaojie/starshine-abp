using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
///实现 <see cref="IFullAuditedObject{TUser}"/> 作为全面审计实体的基类。
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
/// 实现 <see cref="IFullAuditedObjectObject{TUser}"/> 作为全面审计实体的基类。
/// </summary>
/// <typeparam name="TKey">实体主键的类型</typeparam>
/// <typeparam name="TUser">用户类型</typeparam>
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
