using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.SqlSugarCore.Repositories
{
    /// <summary>
    /// SqlSugar仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISqlSugarRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity,new()
    {
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        Task<ISqlSugarClient> GetDbContextAsync();

        /// <summary>
        /// 获取SugarQueryable
        /// </summary>
        /// <returns></returns>
        Task<ISugarQueryable<TEntity>> GetSugarQueryableAsync();

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <returns></returns>
        Task<IDeleteable<TEntity>> GetDeleteableAsync();

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        Task<IInsertable<TEntity>> GetInsertableAsync(IEnumerable<TEntity> insertObjs);

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        Task<IInsertable<TEntity>> GetInsertableAsync(TEntity insertObj);

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        Task<IInsertable<TEntity>> GetInsertableAsync(TEntity[] insertObjs);
        
        /// <summary>
        /// 获取租户
        /// </summary>
        /// <returns></returns>
        Task<ITenant> GetTenantAsync();

        /// <summary>
        /// 获取更新上下文
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        Task<IUpdateable<TEntity>> GetUpdateableAsync(IEnumerable<TEntity> updateObjs);

        /// <summary>
        /// 获取更新上下文
        /// </summary>
        /// <param name="updateObj"></param>
        /// <returns></returns>
        Task<IUpdateable<TEntity>> GetUpdateableAsync(TEntity updateObj);

        /// <summary>
        /// 获取插入操作
        /// </summary>
        /// <returns></returns>
        Task<IUpdateable<TEntity>> GetUpdateableAsync();

        /// <summary>
        /// 获取插入操作
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        Task<IUpdateable<TEntity>> GetUpdateableAsync(TEntity[] updateObjs);
    }

    /// <summary>
    /// SqlSugar仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface ISqlSugarRepository<TEntity, TKey> : ISqlSugarRepository<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {

    }
}
