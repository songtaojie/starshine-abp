using Azure;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.SqlSugarCore.Repositories
{
    /// <summary>
    /// SqlSugar仓储
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class SqlSugarRepository<TEntity> : RepositoryBase<TEntity>, ISqlSugarRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly IDbContextProvider<ISqlSugarDbContext> _dbContextProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public SqlSugarRepository(IDbContextProvider<ISqlSugarDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        /// <summary>
        /// 获取删除操作接口
        /// </summary>
        /// <returns></returns>
        public async Task<IDeleteable<TEntity>> GetDeleteableAsync()
        {
            var context = await GetDbContextAsync();
            return context.Deleteable<TEntity>();
        }

        /// <summary>
        /// 获取插入操作接口
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public async Task<IInsertable<TEntity>> GetInsertableAsync(IEnumerable<TEntity> insertObjs)
        {
            var context = await GetDbContextAsync();
            return context.Insertable<TEntity>(insertObjs.ToList());
        }

        /// <summary>
        /// 获取插入操作接口
        /// </summary>
        /// <param name="insertObj"></param>
        /// <returns></returns>
        public async Task<IInsertable<TEntity>> GetInsertableAsync(TEntity insertObj)
        {
            var context = await GetDbContextAsync();
            return context.Insertable<TEntity>(insertObj);
        }

        /// <summary>
        /// 获取插入操作接口
        /// </summary>
        /// <param name="insertObjs"></param>
        /// <returns></returns>
        public async Task<IInsertable<TEntity>> GetInsertableAsync(TEntity[] insertObjs)
        {
            var context = await GetDbContextAsync();
            return context.Insertable<TEntity>(insertObjs);
        }

        /// <summary>
        /// 获取租户操作
        /// </summary>
        /// <returns></returns>
        public async Task<ITenant> GetTenantAsync()
        {
            var context = await GetDbContextAsync();
            return context.AsTenant();
        }

        /// <summary>
        /// 获取更新操作接口
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public async Task<IUpdateable<TEntity>> GetUpdateableAsync(IEnumerable<TEntity> updateObjs)
        {
            var context = await GetDbContextAsync();
            return context.Updateable<TEntity>(updateObjs.ToList());
        }

        /// <summary>
        /// 获取更新操作接口
        /// </summary>
        /// <param name="updateObj"></param>
        /// <returns></returns>
        public async Task<IUpdateable<TEntity>> GetUpdateableAsync(TEntity updateObj)
        {
            var context = await GetDbContextAsync();
            return context.Updateable<TEntity>(updateObj);
        }

        /// <summary>
        /// 获取更新操作接口
        /// </summary>
        /// <returns></returns>
        public async Task<IUpdateable<TEntity>> GetUpdateableAsync()
        {
            var context = await GetDbContextAsync();
            return context.Updateable<TEntity>();
        }

        /// <summary>
        /// 获取更新操作接口
        /// </summary>
        /// <param name="updateObjs"></param>
        /// <returns></returns>
        public async Task<IUpdateable<TEntity>> GetUpdateableAsync(TEntity[] updateObjs)
        {
            var context = await GetDbContextAsync();
            return context.Updateable<TEntity>(updateObjs);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                var updateable = await GetUpdateableAsync();
                await updateable.Where(predicate)
                    .SetColumns(nameof(ISoftDelete.IsDeleted), true)
                    .ExecuteCommandAsync(GetCancellationToken(cancellationToken));
            }
            else
            {
                var deleteable = await GetDeleteableAsync();
                await deleteable.Where(predicate).ExecuteCommandAsync(GetCancellationToken(cancellationToken));
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                var updateable = await GetUpdateableAsync(entity);
                await updateable
                    .SetColumns(nameof(ISoftDelete.IsDeleted), true)
                    .ExecuteCommandAsync(GetCancellationToken(cancellationToken));
            }
            else
            {
                var deleteable = await GetDeleteableAsync();
                await deleteable.Where(entity).ExecuteCommandAsync(GetCancellationToken(cancellationToken));
            }
        }

        /// <summary>
        /// 直接删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var deleteable = await GetDeleteableAsync();
            await deleteable.Where(predicate).ExecuteCommandAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            return await queryable.FirstAsync(predicate, GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            return await queryable.CountAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 获取SqlSugar客户端对象
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ISqlSugarClient> GetDbContextAsync()
        {
            return (await _dbContextProvider.GetDbContextAsync()).Context;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            return await queryable.ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            return await queryable.Where(predicate).ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="sorting"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            RefAsync<int> count = 0;
            var result = await queryable.ToPageListAsync(skipCount, maxResultCount, count, GetCancellationToken(cancellationToken));
            return result;
        }

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <returns></returns>
        public async Task<ISugarQueryable<TEntity>> GetSugarQueryableAsync()
        {
            var context = await GetDbContextAsync();
            return context.Queryable<TEntity>();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var insertable = await GetInsertableAsync(entity);
            await insertable.ExecuteCommandAsync(GetCancellationToken(cancellationToken));
            return entity;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var updateable = await GetUpdateableAsync(entity);
            await updateable.ExecuteCommandAsync(GetCancellationToken(cancellationToken));
            return entity;
        }

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Obsolete]
        protected override IQueryable<TEntity> GetQueryable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取查询对象
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// SqlSugar仓储类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class SqlSugarRepository<TEntity, TKey> : SqlSugarRepository<TEntity>, ISqlSugarRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// SqlSugar仓储类
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public SqlSugarRepository(IDbContextProvider<ISqlSugarDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 根据主键删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await GetAsync(id, true, cancellationToken);
            await DeleteAsync(entity, autoSave, cancellationToken);
        }

        /// <summary>
        /// 根据主键批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="autoSave"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            var deleteable = await GetDeleteableAsync();
            await deleteable.Where(x=> ids.Contains(x.Id))
                .ExecuteCommandAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 根据主键获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var context = await GetDbContextAsync();
            context.Ado.CancellationToken = GetCancellationToken(cancellationToken);
            return await context.Queryable<TEntity>().InSingleAsync(id);
        }

        /// <summary>
        /// 根据主键获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails, GetCancellationToken(cancellationToken));
            if (entity == null)
                throw new EntityNotFoundException(typeof(TEntity), id);
            return entity;
        }
    }
}
