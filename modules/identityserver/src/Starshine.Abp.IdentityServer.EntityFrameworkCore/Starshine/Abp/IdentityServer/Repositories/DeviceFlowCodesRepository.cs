using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Entities;
using Starshine.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// 设备流代码存储库
/// </summary>
public class DeviceFlowCodesRepository : EfCoreRepository<IIdentityServerDbContext, DeviceFlowCodes, Guid>,
    IDeviceFlowCodesRepository
{
    /// <summary>
    /// 设备流代码存储库
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public DeviceFlowCodesRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }
    /// <summary>
    /// 根据用户代码查找设备流代码
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<DeviceFlowCodes?> FindByUserCodeAsync(string userCode, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(d => d.Id)
            .FirstOrDefaultAsync(d => d.UserCode == userCode, GetCancellationToken(cancellationToken));
    }
    /// <summary>
    /// 根据设备代码查找设备流代码
    /// </summary>
    /// <param name="deviceCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<DeviceFlowCodes?> FindByDeviceCodeAsync(string deviceCode, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(d => d.Id)
            .FirstOrDefaultAsync(d => d.DeviceCode == deviceCode, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 根据过期时间获取设备流代码
    /// </summary>
    /// <param name="maxExpirationDate"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<DeviceFlowCodes>> GetListByExpirationAsync(DateTime maxExpirationDate, int maxResultCount,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Expiration < maxExpirationDate)
            .OrderBy(x => x.ClientId)
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
    /// <summary>
    /// 删除过期的代码
    /// </summary>
    /// <param name="maxExpirationDate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteExpirationAsync(DateTime maxExpirationDate, CancellationToken cancellationToken = default)
    {
        await DeleteDirectAsync(x => x.Expiration < maxExpirationDate, cancellationToken);
    }
}
