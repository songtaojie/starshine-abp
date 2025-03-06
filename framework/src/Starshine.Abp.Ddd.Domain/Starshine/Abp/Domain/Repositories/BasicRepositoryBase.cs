using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Starshine.Abp.Domain.Entities;
using Volo.Abp.Linq;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.Domain.Repositories;
/// <summary>
/// Basic implementation of IBasicRepository.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class BasicRepositoryBase<TEntity> :
    IBasicRepository<TEntity>,
    IUnitOfWorkEnabled
    where TEntity : class, IEntity
{
    /// <summary>
    /// 服务提供商。
    /// </summary>
    public IAbpLazyServiceProvider LazyServiceProvider { get; } 

    /// <summary>
    /// 数据过滤器。
    /// </summary>
    public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    /// <summary>
    /// 当前租户。
    /// </summary>
    public ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    /// <summary>
    /// 异步查询执行器。
    /// </summary>
    public IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

    /// <summary>
    /// 工作单元管理器。
    /// </summary>
    public IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    /// <summary>
    /// 取消令牌提供者。
    /// </summary>
    public ICancellationTokenProvider CancellationTokenProvider => LazyServiceProvider.LazyGetService<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);

    /// <summary>
    /// 日志工厂。
    /// </summary>
    public ILoggerFactory? LoggerFactory => LazyServiceProvider.LazyGetService<ILoggerFactory>();

    /// <summary>
    /// 日志。
    /// </summary>
    public ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);

    /// <summary>
    /// 实体变更跟踪提供者。
    /// </summary>
    public IEntityChangeTrackingProvider EntityChangeTrackingProvider => LazyServiceProvider.LazyGetRequiredService<IEntityChangeTrackingProvider>();

    /// <summary>
    /// 是否跟踪实体变更。
    /// </summary>
    public bool? IsChangeTrackingEnabled { get; protected set; }

    /// <summary>
    /// 构造函数。
    /// </summary>
    /// <param name="abpLazyServiceProvider"></param>
    protected BasicRepositoryBase(IAbpLazyServiceProvider abpLazyServiceProvider)
    {
        LazyServiceProvider = abpLazyServiceProvider;
    }

    /// <summary>
    /// 插入实体。
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<TEntity> InsertAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量插入实体。
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await InsertAsync(entity, cancellationToken: cancellationToken);
        }
        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// 保存更改。
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        if (UnitOfWorkManager?.Current != null)
        {
            return UnitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 更新实体。
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量更新实体。
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await UpdateAsync(entity, cancellationToken: cancellationToken);
        }

        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// 删除实体。
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量删除实体。
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await DeleteAsync(entity, cancellationToken: cancellationToken);
        }

        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// 获取列表。
    /// </summary>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取列表。
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取总数。
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<long> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取分页列表。
    /// </summary>
    /// <param name="skipCount"></param>
    /// <param name="takeCount"></param>
    /// <param name="sorting"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<List<TEntity>> GetPagedListAsync(int skipCount, int takeCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取取消令牌。
    /// </summary>
    /// <param name="preferredValue"></param>
    /// <returns></returns>
    protected virtual CancellationToken GetCancellationToken(CancellationToken preferredValue = default)
    {
        return CancellationTokenProvider.FallbackToProvider(preferredValue);
    }

    /// <summary>
    /// 是否跟踪实体的更改。
    /// </summary>
    /// <returns></returns>
    protected virtual bool ShouldTrackingEntityChange()
    {
        // If IsChangeTrackingEnabled is set, it has the highest priority. This generally means the repository is read-only.
        if (IsChangeTrackingEnabled.HasValue)
        {
            return IsChangeTrackingEnabled.Value;
        }

        // If Interface/Class/Method has Enable/DisableEntityChangeTrackingAttribute, it has the second highest priority.
        if (EntityChangeTrackingProvider.Enabled.HasValue)
        {
            return EntityChangeTrackingProvider.Enabled.Value;
        }

        // Default behavior is tracking entity change.
        return true;
    }
}

/// <summary>
/// 基础仓储基类。
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <remarks>
/// 基础仓储基类。
/// </remarks>
/// <param name="abpLazyServiceProvider"></param>
public abstract class BasicRepositoryBase<TEntity, TKey>(IAbpLazyServiceProvider abpLazyServiceProvider) : BasicRepositoryBase<TEntity>(abpLazyServiceProvider), IBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// 获取实体。
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, includeDetails, cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    /// <summary>
    /// 查找实体。
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除实体。
    /// </summary>
    /// <param name="id"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return;
        }

        await DeleteAsync(entity, autoSave, cancellationToken);
    }

    /// <summary>
    /// 删除多个实体。
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var id in ids)
        {
            await DeleteAsync(id, cancellationToken: cancellationToken);
        }

        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
}
