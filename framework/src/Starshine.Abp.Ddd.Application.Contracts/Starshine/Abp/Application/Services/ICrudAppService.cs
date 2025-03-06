using Starshine.Abp.Application.Dtos;

namespace Starshine.Abp.Application.Services;
/// <summary>
/// CRUDӦ�÷���
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface ICrudAppService<TEntityDto, in TKey>
    : ICrudAppService<TEntityDto, TKey, PagedAndSortedResultRequestDto>
{

}

/// <summary>
/// CRUDӦ�÷���
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TGetListInput"></typeparam>
public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput>
    : ICrudAppService<TEntityDto, TKey, TGetListInput, TEntityDto>
{

}

/// <summary>
///CRUDӦ�÷���
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TGetListInput"></typeparam>
/// <typeparam name="TCreateInput"></typeparam>
public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput>
    : ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
{

}

/// <summary>
/// CRUDӦ�÷���
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TGetListInput"></typeparam>
/// <typeparam name="TCreateInput"></typeparam>
/// <typeparam name="TUpdateInput"></typeparam>
public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
    : ICrudAppService<TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
{

}

/// <summary>
/// CRUDӦ�÷���
/// </summary>
/// <typeparam name="TGetOutputDto"></typeparam>
/// <typeparam name="TGetListOutputDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TGetListInput"></typeparam>
/// <typeparam name="TCreateInput"></typeparam>
/// <typeparam name="TUpdateInput"></typeparam>
public interface ICrudAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
    : IReadOnlyAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>,
        ICreateUpdateAppService<TGetOutputDto, TKey, TCreateInput, TUpdateInput>,
        IDeleteAppService<TKey>
{

}
