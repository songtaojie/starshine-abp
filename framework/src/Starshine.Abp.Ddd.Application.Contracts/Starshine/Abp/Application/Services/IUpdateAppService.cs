using System.Threading.Tasks;

namespace Starshine.Abp.Application.Services;
/// <summary>
/// 更新应用服务
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IUpdateAppService<TEntityDto, in TKey>
    : IUpdateAppService<TEntityDto, TKey, TEntityDto>
{

}

/// <summary>
/// 更新应用服务
/// </summary>
/// <typeparam name="TGetOutputDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TUpdateInput"></typeparam>
public interface IUpdateAppService<TGetOutputDto, in TKey, in TUpdateInput>
    : IApplicationService
{
    Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input);
}
