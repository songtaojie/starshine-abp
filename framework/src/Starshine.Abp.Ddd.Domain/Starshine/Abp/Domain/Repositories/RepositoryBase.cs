using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Starshine.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Abp.Domain.Entities;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Domain.Repositories;

/// <summary>
/// 存储库的基类。
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class RepositoryBase<TEntity>: BasicRepositoryBase<TEntity>, IRepository<TEntity>, IUnitOfWorkManagerAccessor
    where TEntity : class, IEntity
{

    public RepositoryBase(IAbpLazyServiceProvider abpLazyServiceProvider) : base(abpLazyServiceProvider)
    {
    }
    /// <summary>
    /// 获取所有实体的查询。
    /// </summary>
    /// <returns></returns>
    public virtual Task<IQueryable<TEntity>> WithDetailsAsync()
    {
        return GetQueryableAsync();
    }

    /// <summary>
    /// 获取所有实体的查询。
    /// </summary>
    /// <param name="propertySelectors"></param>
    /// <returns></returns>
    public virtual Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        return GetQueryableAsync();
    }

    /// <summary>
    /// 获取所有实体的查询。
    /// </summary>
    /// <returns></returns>
    public abstract Task<IQueryable<TEntity>> GetQueryableAsync();

    /// <summary>
    /// 查找实体。
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate,bool includeDetails = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// 查找实体。
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task<TEntity> GetAsync( Expression<Func<TEntity, bool>> predicate,bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(predicate, includeDetails, cancellationToken);

        return entity ?? throw new EntityNotFoundException(typeof(TEntity));
    }

    /// <summary>
    /// 删除实体。
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除实体。
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// 应用数据过滤器。
    /// </summary>
    /// <typeparam name="TQueryable"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    protected virtual TQueryable ApplyDataFilters<TQueryable>(TQueryable query)
        where TQueryable : IQueryable<TEntity>
    {
        return ApplyDataFilters<TQueryable, TEntity>(query);
    }

    /// <summary>
    /// 应用数据过滤器。
    /// </summary>
    /// <typeparam name="TQueryable"></typeparam>
    /// <typeparam name="TOtherEntity"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    protected virtual TQueryable ApplyDataFilters<TQueryable, TOtherEntity>(TQueryable query)
        where TQueryable : IQueryable<TOtherEntity>
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TOtherEntity)))
        {
            query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<ISoftDelete>(), e => ((ISoftDelete)e!).IsDeleted == false);
        }

        if (typeof(IMultiTenant).IsAssignableFrom(typeof(TOtherEntity)))
        {
            var tenantId = CurrentTenant.Id;
            query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<IMultiTenant>(), e => ((IMultiTenant)e!).TenantId == tenantId);
        }

        return query;
    }
}

/// <summary>
/// 存储库的基类。
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public abstract class RepositoryBase<TEntity, TKey> : RepositoryBase<TEntity>, IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public RepositoryBase(IAbpLazyServiceProvider abpLazyServiceProvider) : base(abpLazyServiceProvider)
    {
    }
    /// <summary>
    /// 查找实体。
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default);

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
    public virtual async Task DeleteAsync(TKey id, bool autoSave = true, CancellationToken cancellationToken = default)
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
    public async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = true, CancellationToken cancellationToken = default)
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
