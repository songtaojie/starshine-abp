using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// 此类的类型可用于简化实现<see cref="IAuditedObject{TUser}"/>的聚合根。实体的主键
/// </summary>
/// <typeparam name="TUser">用户类型</typeparam>
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
/// 此类的类型可用于简化实现<see cref="IAuditedObject{TUser}"/>的聚合根。实体的主键
/// </summary>
/// <typeparam name="TKey">实体主键的类型</typeparam>
/// <typeparam name="TUser">用户类型</typeparam>
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
