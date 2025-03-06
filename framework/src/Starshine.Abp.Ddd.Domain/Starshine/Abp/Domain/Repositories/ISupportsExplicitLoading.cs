using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Starshine.Abp.Domain.Entities;

namespace Starshine.Abp.Domain.Repositories;
/// <summary>
/// 支持显式加载
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface ISupportsExplicitLoading<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// 确保集合加载
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="entity"></param>
    /// <param name="propertyExpression"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task EnsureCollectionLoadedAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken)
        where TProperty : class;
    /// <summary>
    /// 确保属性加载
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="entity"></param>
    /// <param name="propertyExpression"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task EnsurePropertyLoadedAsync<TProperty>(
        TEntity entity,
        Expression<Func<TEntity, TProperty?>> propertyExpression,
        CancellationToken cancellationToken)
        where TProperty : class;
}
