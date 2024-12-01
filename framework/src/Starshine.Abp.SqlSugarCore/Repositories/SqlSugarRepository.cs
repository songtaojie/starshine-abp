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
   
    public class SqlSugarRepository<TEntity> : RepositoryBase<TEntity>, ISqlSugarRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly IDbContextProvider<ISqlSugarDbContext> _dbContextProvider;
        public SqlSugarRepository(IDbContextProvider<ISqlSugarDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public async Task<IDeleteable<TEntity>> GetDeleteableAsync()
        {
            var context = await GetDbContextAsync();
            return context.Deleteable<TEntity>();
        }

        public async Task<IInsertable<TEntity>> GetInsertableAsync(IEnumerable<TEntity> insertObjs)
        {
            var context = await GetDbContextAsync();
            return context.Insertable<TEntity>(insertObjs.ToList());
        }

        public async Task<IInsertable<TEntity>> GetInsertableAsync(TEntity insertObj)
        {
            var context = await GetDbContextAsync();
            return context.Insertable<TEntity>(insertObj);
        }

        public async Task<IInsertable<TEntity>> GetInsertableAsync(TEntity[] insertObjs)
        {
            var context = await GetDbContextAsync();
            return context.Insertable<TEntity>(insertObjs);
        }

        public async Task<ITenant> GetTenantAsync()
        {
            var context = await GetDbContextAsync();
            return context.AsTenant();
        }

        public async Task<IUpdateable<TEntity>> GetUpdateableAsync(IEnumerable<TEntity> updateObjs)
        {
            var context = await GetDbContextAsync();
            return context.Updateable<TEntity>(updateObjs.ToList());
        }

        public async Task<IUpdateable<TEntity>> GetUpdateableAsync(TEntity updateObj)
        {
            var context = await GetDbContextAsync();
            return context.Updateable<TEntity>(updateObj);
        }

        public async Task<IUpdateable<TEntity>> GetUpdateableAsync()
        {
            var context = await GetDbContextAsync();
            return context.Updateable<TEntity>();
        }

        public async Task<IUpdateable<TEntity>> GetUpdateableAsync(TEntity[] updateObjs)
        {
            var context = await GetDbContextAsync();
            return context.Updateable<TEntity>(updateObjs);
        }

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

        public override async Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var deleteable = await GetDeleteableAsync();
            await deleteable.Where(predicate).ExecuteCommandAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            return await queryable.FirstAsync(predicate, GetCancellationToken(cancellationToken));
        }

        public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            return await queryable.CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<ISqlSugarClient> GetDbContextAsync()
        {
            return (await _dbContextProvider.GetDbContextAsync()).Context;
        }

        public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            return await queryable.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            return await queryable.Where(predicate).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<List<TEntity>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = await GetSugarQueryableAsync();
            RefAsync<int> count = 0;
            var result = await queryable.ToPageListAsync(skipCount, maxResultCount, count, GetCancellationToken(cancellationToken));
            return result;
        }

        public async Task<ISugarQueryable<TEntity>> GetSugarQueryableAsync()
        {
            var context = await GetDbContextAsync();
            return context.Queryable<TEntity>();
        }

        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var insertable = await GetInsertableAsync(entity);
            await insertable.ExecuteCommandAsync(GetCancellationToken(cancellationToken));
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var updateable = await GetUpdateableAsync(entity);
            await updateable.ExecuteCommandAsync(GetCancellationToken(cancellationToken));
            return entity;
        }

        [Obsolete]
        protected override IQueryable<TEntity> GetQueryable()
        {
            throw new NotImplementedException();
        }
        public override Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class SqlSugarRepository<TEntity, TKey> : SqlSugarRepository<TEntity>, ISqlSugarRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>, new()
    {
        public SqlSugarRepository(IDbContextProvider<ISqlSugarDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var updateable = await GetUpdateableAsync();
            var entity = await GetAsync(id, true, cancellationToken);
            await DeleteAsync(entity, autoSave, cancellationToken);
        }

        public async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            var deleteable = await GetDeleteableAsync();
            await deleteable.Where(x=> ids.Contains(x.Id))
                .ExecuteCommandAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var context = await GetDbContextAsync();
            context.Ado.CancellationToken = GetCancellationToken(cancellationToken);
            return await context.Queryable<TEntity>().InSingleAsync(id);
        }

        public async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails, GetCancellationToken(cancellationToken));
            if (entity == null)
                throw new EntityNotFoundException(typeof(TEntity), id);
            return entity;
        }
    }
}
