using System.Threading.Tasks;
using Starshine.IdentityServer.Configuration;
using Starshine.IdentityServer.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.ObjectMapping;

namespace Starshine.Abp.IdentityServer.Clients;

/// <summary>
/// 客户端存储
/// </summary>
public class ClientStore : IClientStore
{
    /// <summary>
    /// 客户端存储
    /// </summary>
    protected IClientRepository ClientRepository { get; }
    protected IObjectMapper<StarshineIdentityServerDomainModule> ObjectMapper { get; }
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
    /// <param name="objectMapper"></param>
    /// <param name="cache"></param>
    /// <param name="options"></param>
    public ClientStore(
        IClientRepository clientRepository,
        IObjectMapper<StarshineIdentityServerDomainModule> objectMapper,
        IDistributedCache<Starshine.IdentityServer.Models.Client> cache,
        IOptions<IdentityServerOptions> options)
    {
        ClientRepository = clientRepository;
        ObjectMapper = objectMapper;
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
    protected virtual async Task<Starshine.IdentityServer.Models.Client> GetCacheItemAsync(string clientId)
    {
        return (await Cache.GetOrAddAsync(clientId, async () =>
            {
                var client = await ClientRepository.FindByClientIdAsync(clientId);
                return ObjectMapper.Map<Client, Starshine.IdentityServer.Models.Client>(client!);
            },
            optionsFactory: () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
            },
            considerUow: true))!;

    }
}
