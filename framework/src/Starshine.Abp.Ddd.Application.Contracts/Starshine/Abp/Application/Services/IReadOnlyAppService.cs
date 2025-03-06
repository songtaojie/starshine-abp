using System.Threading.Tasks;
using Starshine.Abp.Application.Dtos;

namespace Starshine.Abp.Application.Services;
/// <summary>
/// 只读应用服务
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IReadOnlyAppService<TEntityDto, in TKey>
    : IReadOnlyAppService<TEntityDto, TEntityDto, TKey, PagedAndSortedResultRequestDto>
{

}

/// <summary>
/// 只读应用服务
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TGetListInput"></typeparam>
public interface IReadOnlyAppService<TEntityDto, in TKey, in TGetListInput>
    : IReadOnlyAppService<TEntityDto, TEntityDto, TKey, TGetListInput>
{

}

/// <summary>
/// 只读应用服务
/// </summary>
/// <typeparam name="TGetOutputDto"></typeparam>
/// <typeparam name="TGetListOutputDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TGetListInput"></typeparam>
public interface IReadOnlyAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput>
    : IApplicationService
{
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TGetOutputDto> GetAsync(TKey id);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input);
}
