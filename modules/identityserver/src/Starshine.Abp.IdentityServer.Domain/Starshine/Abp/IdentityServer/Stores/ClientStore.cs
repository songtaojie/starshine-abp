using System.Threading.Tasks;
using Starshine.IdentityServer.Configuration;
using Starshine.IdentityServer.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.ObjectMapping;
using Starshine.Abp.IdentityServer.Repositories;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer.Stores;

/// <summary>
/// 客户端存储
/// </summary>
public class ClientStore : IClientStore
{
    /// <summary>
    /// 客户端存储
    /// </summary>
    protected IClientRepository ClientRepository { get; }
    /// <summary>
    /// 缓存
    /// </summary>
    protected IDistributedCache<Starshine.IdentityServer.Models.Client> Cache { get; }
    /// <summary>
    /// 配置
    /// </summary>
    protected IdentityServerOptions Options { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="clientRepository"></param>
    /// <param name="cache"></param>
    /// <param name="options"></param>
    public ClientStore(
        IClientRepository clientRepository,
        IDistributedCache<Starshine.IdentityServer.Models.Client> cache,
        IOptions<IdentityServerOptions> options)
    {
        ClientRepository = clientRepository;
        Cache = cache;
        Options = options.Value;
    }

    /// <summary>
    /// 根据客户端ID查找客户端
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    public virtual async Task<Starshine.IdentityServer.Models.Client?> FindClientByIdAsync(string clientId)
    {
        return await GetCacheItemAsync(clientId);
    }

    /// <summary>
    /// 获取缓存项
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    protected virtual async Task<Starshine.IdentityServer.Models.Client?> GetCacheItemAsync(string clientId)
    {
        return await Cache.GetOrAddAsync(clientId, async () =>
            {
                var client = await ClientRepository.FindByClientIdAsync(clientId);
                return client?.ToClientModel()!;
            },
            optionsFactory: () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
            },
            considerUow: true);

    }
}
