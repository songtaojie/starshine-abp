using System.Threading.Tasks;

namespace Starshine.Abp.Application.Services;

/// <summary>
/// 删除应用服务
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IDeleteAppService<in TKey> : IApplicationService
{
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(TKey id);
}
