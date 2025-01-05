using System.Threading.Tasks;
using Starshine.IdentityServer.Configuration;
using Starshine.IdentityServer.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.ObjectMapping;

namespace Starshine.Abp.IdentityServer.Clients;

public class ClientStore : IClientStore
{
    protected IClientRepository ClientRepository { get; }
    protected IObjectMapper<StarshineIdentityServerDomainModule> ObjectMapper { get; }
    protected IDistributedCache<Starshine.IdentityServer.Models.Client> Cache { get; }
    protected IdentityServerOptions Options { get; }

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

    public virtual async Task<Starshine.IdentityServer.Models.Client> FindClientByIdAsync(string clientId)
    {
        return await GetCacheItemAsync(clientId);
    }

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
