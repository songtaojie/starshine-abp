using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// 设备流代码存储库
/// </summary>
public interface IDeviceFlowCodesRepository : IBasicRepository<DeviceFlowCodes, Guid>
{
    /// <summary>
    /// 通过用户代码查找设备流代码
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeviceFlowCodes?> FindByUserCodeAsync(string userCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// 通过设备代码查找设备流代码
    /// </summary>
    /// <param name="deviceCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeviceFlowCodes?> FindByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// 通过过期时间查找设备流代码
    /// </summary>
    /// <param name="maxExpirationDate"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<DeviceFlowCodes>> GetListByExpirationAsync(DateTime maxExpirationDate, int maxResultCount, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除过期的
    /// </summary>
    /// <param name="maxExpirationDate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteExpirationAsync(DateTime maxExpirationDate, CancellationToken cancellationToken = default);
}
