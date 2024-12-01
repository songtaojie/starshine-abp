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
    public interface ISqlSugarRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity,new()
    {
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
        
        Task<ITenant> GetTenantAsync();

        Task<IUpdateable<TEntity>> GetUpdateableAsync(IEnumerable<TEntity> updateObjs);

        Task<IUpdateable<TEntity>> GetUpdateableAsync(TEntity updateObj);

        Task<IUpdateable<TEntity>> GetUpdateableAsync();

        Task<IUpdateable<TEntity>> GetUpdateableAsync(TEntity[] updateObjs);
    }

    public interface ISqlSugarRepository<TEntity, TKey> : ISqlSugarRepository<TEntity>, IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {

    }
}
