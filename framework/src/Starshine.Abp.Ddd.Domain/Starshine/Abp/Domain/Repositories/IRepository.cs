using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Starshine.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.Domain.Repositories;

/// <summary>
/// 只是为了将一个类标记为存储库。
/// </summary>
public interface IRepository
{
    /// <summary>
    /// 是否已启用更改跟踪
    /// </summary>
    bool? IsChangeTrackingEnabled { get; }
}

/// <summary>
/// 存储库
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>, IBasicRepository<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Get a single entity by the given <paramref name="predicate"/>.
    /// <para>
    /// It returns null if there is no entity with the given <paramref name="predicate"/>.
    /// It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
    /// </para>
    /// </summary>
    /// <param name="predicate">A condition to find the entity</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default );

    /// <summary>
    /// Get a single entity by the given <paramref name="predicate"/>.
    /// <para>
    /// It throws <see cref="EntityNotFoundException"/> if there is no entity with the given <paramref name="predicate"/>.
    /// It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
    /// </para>
    /// </summary>
    /// <param name="predicate">A condition to filter entities</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default );

    /// <summary>
    /// Deletes many entities by the given <paramref name="predicate"/>.
    /// <para>
    /// Please note: This may cause major performance problems if there are too many entities returned for a
    /// given predicate and the database provider doesn't have a way to efficiently delete many entities.
    /// </para>
    /// </summary>
    /// <param name="predicate">A condition to filter entities</param>
    /// <param name="autoSave">
    /// 设置为 true 则自动保存对数据库的改变。
    /// 这对于 ORM/数据库 API 很有用，它们仅通过显式方法调用保存更改，但您需要立即将更改保存到数据库。
    /// </param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,bool autoSave = true,CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all entities those fit to the given predicate.
    /// It directly deletes entities from database, without fetching them.
    /// Some features (like soft-delete, multi-tenancy and audit logging) won't work, so use this method carefully when you need it.
    /// Use the DeleteAsync method if you need to these features.
    /// </summary>
    /// <param name="predicate">A condition to filter entities</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns></returns>
    Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate,CancellationToken cancellationToken = default);
}

/// <summary>
/// 存储库
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IRepository<TEntity, TKey> : IRepository<TEntity>, IReadOnlyRepository<TEntity, TKey>, IBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}
