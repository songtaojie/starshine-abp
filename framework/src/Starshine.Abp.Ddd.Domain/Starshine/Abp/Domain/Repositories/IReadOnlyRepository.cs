using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Starshine.Abp.Domain.Entities;
using Volo.Abp.Linq;

namespace Starshine.Abp.Domain.Repositories;

/// <summary>
/// <typeparamref name="TEntity"/> 的只读存储库接口。
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IReadOnlyRepository<TEntity> : IReadOnlyBasicRepository<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// 异步执行器
    /// </summary>
    IAsyncQueryableExecuter AsyncExecuter { get; }

    Task<IQueryable<TEntity>> WithDetailsAsync(); //TODO: CancellationToken

    Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors); //TODO: CancellationToken

    Task<IQueryable<TEntity>> GetQueryableAsync(); //TODO: CancellationToken

    /// <summary>
    /// Gets a list of entities by the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A condition to filter the entities</param>
    /// <param name="includeDetails">Set true to include details (sub-collections) of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<List<TEntity>> GetListAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}

public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{

}
