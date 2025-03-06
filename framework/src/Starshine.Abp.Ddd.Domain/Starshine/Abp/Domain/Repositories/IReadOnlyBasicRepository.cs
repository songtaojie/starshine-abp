// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using JetBrains.Annotations;
using Starshine.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.Domain.Repositories;

/// <summary>
/// 定义对实体进行只读操作的基本方法。
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IReadOnlyBasicRepository<TEntity> : IRepository where TEntity : class, IEntity
{
    /// <summary>
    /// 获取所有实体的列表。
    /// </summary>
    /// <param name="includeDetails">设置为 true 以包含此实体的所有子项</param>
    /// <param name="cancellationToken">等待任务完成时观察 <see cref="T:System.Threading.CancellationToken" />。</param>
    /// <returns>Entity</returns>
    Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    ///获取所有实体的总数。
    /// </summary>
    Task<long> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <param name="skipCount"></param>
    /// <param name="takeCount"></param>
    /// <param name="sorting"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TEntity>> GetPagedListAsync(int skipCount, int takeCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default);
}

/// <summary>
/// 定义对实体进行只读操作的基本方法。
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IReadOnlyBasicRepository<TEntity, TKey> : IReadOnlyBasicRepository<TEntity> where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// Gets an entity with given primary key.
    /// Throws <see cref="EntityNotFoundException"/> if can not find an entity with given id.
    /// </summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity</returns>
    Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity with given primary key or null if not found.
    /// </summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity or null</returns>
    Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);
}
