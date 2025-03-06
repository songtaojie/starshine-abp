using System.Threading.Tasks;

namespace Starshine.Abp.Application.Services;

/// <summary>
/// 创建应用服务
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
public interface ICreateAppService<TEntityDto>
    : ICreateAppService<TEntityDto, TEntityDto>
{

}

/// <summary>
/// 创建应用服务
/// </summary>
/// <typeparam name="TGetOutputDto"></typeparam>
/// <typeparam name="TCreateInput"></typeparam>
public interface ICreateAppService<TGetOutputDto, in TCreateInput>
    : IApplicationService
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<TGetOutputDto> CreateAsync(TCreateInput input);
}
