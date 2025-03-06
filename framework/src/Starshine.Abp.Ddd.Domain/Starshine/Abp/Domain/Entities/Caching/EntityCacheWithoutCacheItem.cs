using Volo.Abp.Caching;
using Starshine.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Starshine.Abp.Domain.Entities.Caching;

public class EntityCacheWithoutCacheItem<TEntity, TKey> :
    EntityCacheBase<TEntity, TEntity, TKey>
    where TEntity : Entity<TKey>
{
    public EntityCacheWithoutCacheItem(
        IReadOnlyRepository<TEntity, TKey> repository,
        IDistributedCache<TEntity, TKey> cache,
        IUnitOfWorkManager unitOfWorkManager)
        : base(repository, cache, unitOfWorkManager)
    {
    }

    protected override TEntity? MapToCacheItem(TEntity? entity)
    {
        return entity;
    }
}
