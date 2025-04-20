using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
///实现 <see cref="IFullAuditedObject{TUser}"/> 作为完全审计聚合根的基类。
/// </summary>
/// <typeparam name="TUser">用户类型</typeparam>
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
/// 实现 <see cref="IFullAuditedObject{TUser}"/> 作为完全审计聚合根的基类。
/// </summary>
/// <typeparam name="TKey">实体主键的类型</typeparam>
/// <typeparam name="TUser">用户类型</typeparam>
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
