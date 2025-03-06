using Volo.Abp.Caching;
using Starshine.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Starshine.Abp.Domain.Entities.Caching;

public class EntityCacheWithObjectMapperContext<TObjectMapperContext, TEntity, TEntityCacheItem, TKey> :
    EntityCacheWithObjectMapper<TEntity, TEntityCacheItem, TKey>
    where TEntity : Entity<TKey>
    where TEntityCacheItem : class
{
    public EntityCacheWithObjectMapperContext(
        IReadOnlyRepository<TEntity, TKey> repository,
        IDistributedCache<TEntityCacheItem, TKey> cache,
        IUnitOfWorkManager unitOfWorkManager,
        IObjectMapper objectMapper)// Intentionally injected with TContext
        : base(repository, cache, unitOfWorkManager, objectMapper)
    {

    }
}
