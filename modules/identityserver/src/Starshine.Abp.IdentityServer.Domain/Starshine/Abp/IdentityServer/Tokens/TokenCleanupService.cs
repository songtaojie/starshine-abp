using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Starshine.Abp.IdentityServer.Repositories;

namespace Starshine.Abp.IdentityServer.Tokens;
/// <summary>
///令牌清理服务
/// </summary>
public class TokenCleanupService : ITransientDependency
{
    /// <summary>
    /// 持久授权存储库
    /// </summary>
    protected IPersistentGrantRepository PersistentGrantRepository { get; }

    /// <summary>
    /// 设备授权存储库
    /// </summary>
    protected IDeviceFlowCodesRepository DeviceFlowCodesRepository { get; }

    /// <summary>
    /// 令牌清理选项
    /// </summary>
    protected TokenCleanupOptions Options { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="persistentGrantRepository"></param>
    /// <param name="deviceFlowCodesRepository"></param>
    /// <param name="options"></param>
    public TokenCleanupService(
        IPersistentGrantRepository persistentGrantRepository,
        IDeviceFlowCodesRepository deviceFlowCodesRepository,
        IOptions<TokenCleanupOptions> options)
    {
        PersistentGrantRepository = persistentGrantRepository;
        DeviceFlowCodesRepository = deviceFlowCodesRepository;
        Options = options.Value;
    }

    /// <summary>
    /// 清理令牌
    /// </summary>
    /// <returns></returns>
    [UnitOfWork]
    public virtual async Task CleanAsync()
    {
        await RemoveGrantsAsync();
        await RemoveDeviceCodesAsync();
    }

    /// <summary>
    /// 移除授权
    /// </summary>
    /// <returns></returns>
    protected virtual async Task RemoveGrantsAsync()
    {
        await PersistentGrantRepository.DeleteExpirationAsync(DateTime.UtcNow);
    }

    /// <summary>
    /// 移除设备授权
    /// </summary>
    /// <returns></returns>
    protected virtual async Task RemoveDeviceCodesAsync()
    {
        await DeviceFlowCodesRepository.DeleteExpirationAsync(DateTime.UtcNow);
    }
}
