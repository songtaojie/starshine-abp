using Starshine.Abp.Application.Dtos;
using Starshine.Abp.Domain.Entities;
using Starshine.Abp.Domain.Repositories;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Application.Services;

public abstract class ReadOnlyAppService<TEntity, TEntityDto, TKey>
    : ReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, PagedAndSortedResultRequestDto>
    where TEntity : class, IEntity<TKey>
{
    protected ReadOnlyAppService(IReadOnlyRepository<TEntity, TKey> repository, IAbpLazyServiceProvider lazyServiceProvider)
        : base(repository, lazyServiceProvider)
    {

    }
}

public abstract class ReadOnlyAppService<TEntity, TEntityDto, TKey, TGetListInput>
    : ReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput>
    where TEntity : class, IEntity<TKey>
{
    protected ReadOnlyAppService(IReadOnlyRepository<TEntity, TKey> repository, IAbpLazyServiceProvider lazyServiceProvider)
        : base(repository, lazyServiceProvider)
    {

    }
}

public abstract class ReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>
    : AbstractKeyReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>
    where TEntity : class, IEntity<TKey>
{
    protected IReadOnlyRepository<TEntity, TKey> Repository { get; }

    protected ReadOnlyAppService(IReadOnlyRepository<TEntity, TKey> repository, IAbpLazyServiceProvider lazyServiceProvider)
    : base(repository, lazyServiceProvider)
    {
        Repository = repository;
    }

    protected override async Task<TEntity> GetEntityByIdAsync(TKey id)
    {
        return await Repository.GetAsync(id);
    }

    protected override IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
    {
        if (typeof(TEntity).IsAssignableTo<ICreationAuditedObject>())
        {
            return query.OrderByDescending(e => ((ICreationAuditedObject)e).CreationTime);
        }
        else
        {
            return query.OrderByDescending(e => e.Id);
        }
    }
}
