using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Starshine.IdentityServer.Stores;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace Starshine.Abp.IdentityServer.Grants;

public class PersistedGrantStore : IPersistedGrantStore
{
    protected IPersistentGrantRepository PersistentGrantRepository { get; }
    protected IObjectMapper<StarshineIdentityServerDomainModule> ObjectMapper { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public PersistedGrantStore(IPersistentGrantRepository persistentGrantRepository,
        IObjectMapper<StarshineIdentityServerDomainModule> objectMapper, IGuidGenerator guidGenerator)
    {
        PersistentGrantRepository = persistentGrantRepository;
        ObjectMapper = objectMapper;
        GuidGenerator = guidGenerator;
    }

    public virtual async Task StoreAsync(Starshine.IdentityServer.Models.PersistedGrant grant)
    {
        var entity = await PersistentGrantRepository.FindByKeyAsync(grant.Key);
        if (entity == null)
        {
            entity = ObjectMapper.Map<Starshine.IdentityServer.Models.PersistedGrant, PersistedGrant>(grant);
            EntityHelper.TrySetId(entity, () => GuidGenerator.Create());
            await PersistentGrantRepository.InsertAsync(entity);
        }
        else
        {
            ObjectMapper.Map(grant, entity);
            await PersistentGrantRepository.UpdateAsync(entity);
        }
    }

    public virtual async Task<Starshine.IdentityServer.Models.PersistedGrant> GetAsync(string key)
    {
        var persistedGrant = await PersistentGrantRepository.FindByKeyAsync(key);
        return ObjectMapper.Map<PersistedGrant, Starshine.IdentityServer.Models.PersistedGrant>(persistedGrant!);
    }

    public virtual async Task<IEnumerable<Starshine.IdentityServer.Models.PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
    {
        var persistedGrants = await PersistentGrantRepository.GetListAsync(filter.SubjectId, filter.SessionId, filter.ClientId, filter.Type);
        return ObjectMapper.Map<List<PersistedGrant>, List<Starshine.IdentityServer.Models.PersistedGrant>>(persistedGrants);
    }

    public virtual async Task RemoveAsync(string key)
    {
        var persistedGrant = await PersistentGrantRepository.FindByKeyAsync(key);
        if (persistedGrant == null)
        {
            return;
        }

        await PersistentGrantRepository.DeleteAsync(persistedGrant);
    }

    public virtual async Task RemoveAllAsync(PersistedGrantFilter filter)
    {
        await PersistentGrantRepository.DeleteAsync(filter.SubjectId, filter.SessionId, filter.ClientId, filter.Type);
    }
}
