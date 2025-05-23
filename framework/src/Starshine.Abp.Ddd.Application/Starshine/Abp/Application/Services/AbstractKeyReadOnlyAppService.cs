﻿using System.Linq.Dynamic.Core;
using Starshine.Abp.Application.Dtos;
using Starshine.Abp.Domain.Entities;
using Starshine.Abp.Domain.Repositories;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Starshine.Abp.Application.Services;

public abstract class AbstractKeyReadOnlyAppService<TEntity, TEntityDto, TKey>
    : AbstractKeyReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, PagedAndSortedResultRequestDto>
    where TEntity : class, IEntity
{
    protected AbstractKeyReadOnlyAppService(IReadOnlyRepository<TEntity> repository,IAbpLazyServiceProvider lazyServiceProvider)
        : base(repository, lazyServiceProvider)
    {

    }
}

public abstract class AbstractKeyReadOnlyAppService<TEntity, TEntityDto, TKey, TGetListInput>
    : AbstractKeyReadOnlyAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput>
    where TEntity : class, IEntity
{
    protected AbstractKeyReadOnlyAppService(IReadOnlyRepository<TEntity> repository, IAbpLazyServiceProvider lazyServiceProvider)
        : base(repository, lazyServiceProvider)
    {

    }
}

public abstract class AbstractKeyReadOnlyAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>
    : ApplicationService
    , IReadOnlyAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>
    where TEntity : class, IEntity
{
    protected IReadOnlyRepository<TEntity> ReadOnlyRepository { get; }

    protected virtual string? GetPolicyName { get; set; }

    protected virtual string? GetListPolicyName { get; set; }

    protected AbstractKeyReadOnlyAppService(IReadOnlyRepository<TEntity> repository,IAbpLazyServiceProvider abpLazyServiceProvider):base(abpLazyServiceProvider)
    {
        ReadOnlyRepository = repository;
    }

    public virtual async Task<TGetOutputDto> GetAsync(TKey id)
    {
        await CheckGetPolicyAsync();

        var entity = await GetEntityByIdAsync(id);

        return await MapToGetOutputDtoAsync(entity);
    }

    public virtual async Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
    {
        await CheckGetListPolicyAsync();

        var query = await CreateFilteredQueryAsync(input);
        var totalCount = await AsyncExecuter.CountAsync(query);

        var entities = new List<TEntity>();
        var entityDtos = new List<TGetListOutputDto>();

        if (totalCount > 0)
        {
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            entities = await AsyncExecuter.ToListAsync(query);
            entityDtos = await MapToGetListOutputDtosAsync(entities);
        }

        return new PagedResultDto<TGetListOutputDto>(
            totalCount,
            entityDtos
        );
    }

    protected abstract Task<TEntity> GetEntityByIdAsync(TKey id);

    protected virtual async Task CheckGetPolicyAsync()
    {
        await CheckPolicyAsync(GetPolicyName);
    }

    protected virtual async Task CheckGetListPolicyAsync()
    {
        await CheckPolicyAsync(GetListPolicyName);
    }

    /// <summary>
    /// Should apply sorting if needed.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="input">The input.</param>
    protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetListInput input)
    {
        //Try to sort query if available
        if (input is ISortedResultRequest sortInput)
        {
            if (!sortInput.Sorting.IsNullOrWhiteSpace())
            {
                return query.OrderBy(sortInput.Sorting!);
            }
        }

        //IQueryable.Task requires sorting, so we should sort if Take will be used.
        if (input is ILimitedResultRequest)
        {
            return ApplyDefaultSorting(query);
        }

        //No sorting
        return query;
    }

    /// <summary>
    /// Applies sorting if no sorting specified but a limited result requested.
    /// </summary>
    /// <param name="query">The query.</param>
    protected virtual IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
    {
        if (typeof(TEntity).IsAssignableTo<IHasCreationTime>())
        {
            return query.OrderByDescending(e => ((IHasCreationTime)e).CreationTime);
        }

        throw new AbpException("No sorting specified but this query requires sorting. Override the ApplySorting or the ApplyDefaultSorting method for your application service derived from AbstractKeyReadOnlyAppService!");
    }

    /// <summary>
    /// Should apply paging if needed.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="input">The input.</param>
    protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetListInput input)
    {
        //Try to use paging if available
        if (input is IPagedResultRequest pagedInput)
        {
            return query.PageBy(pagedInput);
        }

        //Try to limit query result if available
        if (input is ILimitedResultRequest limitedInput)
        {
            return query.Take(limitedInput.MaxResultCount);
        }

        //No paging
        return query;
    }

    /// <summary>
    /// This method should create <see cref="IQueryable{TEntity}"/> based on given input.
    /// It should filter query if needed, but should not do sorting or paging.
    /// Sorting should be done in <see cref="ApplySorting"/> and paging should be done in <see cref="ApplyPaging"/>
    /// methods.
    /// </summary>
    /// <param name="input">The input.</param>
    protected virtual async Task<IQueryable<TEntity>> CreateFilteredQueryAsync(TGetListInput input)
    {
        return await ReadOnlyRepository.GetQueryableAsync();
    }

    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetOutputDto"/>.
    /// It internally calls the <see cref="MapToGetOutputDto"/> by default.
    /// It can be overriden for custom mapping.
    /// Overriding this has higher priority than overriding the <see cref="MapToGetOutputDto"/>
    /// </summary>
    protected virtual Task<TGetOutputDto> MapToGetOutputDtoAsync(TEntity entity)
    {
        return Task.FromResult(MapToGetOutputDto(entity));
    }

    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetOutputDto"/>.
    /// It uses <see cref="IObjectMapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual TGetOutputDto MapToGetOutputDto(TEntity entity)
    {
        return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
    }

    /// <summary>
    /// Maps a list of <typeparamref name="TEntity"/> to <typeparamref name="TGetListOutputDto"/> objects.
    /// It uses <see cref="MapToGetListOutputDtoAsync"/> method for each item in the list.
    /// </summary>
    protected virtual async Task<List<TGetListOutputDto>> MapToGetListOutputDtosAsync(List<TEntity> entities)
    {
        var dtos = new List<TGetListOutputDto>();

        foreach (var entity in entities)
        {
            dtos.Add(await MapToGetListOutputDtoAsync(entity));
        }

        return dtos;
    }

    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetListOutputDto"/>.
    /// It internally calls the <see cref="MapToGetListOutputDto"/> by default.
    /// It can be overriden for custom mapping.
    /// Overriding this has higher priority than overriding the <see cref="MapToGetListOutputDto"/>
    /// </summary>
    protected virtual Task<TGetListOutputDto> MapToGetListOutputDtoAsync(TEntity entity)
    {
        return Task.FromResult(MapToGetListOutputDto(entity));
    }

    /// <summary>
    /// Maps <typeparamref name="TEntity"/> to <typeparamref name="TGetListOutputDto"/>.
    /// It uses <see cref="IObjectMapper"/> by default.
    /// It can be overriden for custom mapping.
    /// </summary>
    protected virtual TGetListOutputDto MapToGetListOutputDto(TEntity entity)
    {
        return ObjectMapper.Map<TEntity, TGetListOutputDto>(entity);
    }
}
