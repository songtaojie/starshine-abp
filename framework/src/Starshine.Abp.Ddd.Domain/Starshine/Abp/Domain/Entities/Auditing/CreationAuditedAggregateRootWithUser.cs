using System;
using Volo.Abp.Auditing;

namespace Starshine.Abp.Domain.Entities.Auditing;

/// <summary>
/// 此类可用于简化聚合根的 <see cref="ICreationAuditedObject{TCreator}"/> 实现。
/// </summary>
/// <typeparam name="TUser">用户类型</typeparam>
[Serializable]
public abstract class CreationAuditedAggregateRootWithUser<TUser> : CreationAuditedAggregateRoot, ICreationAuditedObject<TUser>
{
    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }
}

/// <summary>
/// 此类可用于简化聚合根的 <see cref="ICreationAuditedObject{TCreator}"/> 实现。
/// </summary>
/// <typeparam name="TKey">实体主键的类型</typeparam>
/// <typeparam name="TUser">用户类型</typeparam>
[Serializable]
public abstract class CreationAuditedAggregateRootWithUser<TKey, TUser> : CreationAuditedAggregateRoot<TKey>, ICreationAuditedObject<TUser>
{
    /// <inheritdoc />
    public virtual TUser? Creator { get; protected set; }

    protected CreationAuditedAggregateRootWithUser()
    {

    }

    protected CreationAuditedAggregateRootWithUser(TKey id)
        : base(id)
    {

    }
}
