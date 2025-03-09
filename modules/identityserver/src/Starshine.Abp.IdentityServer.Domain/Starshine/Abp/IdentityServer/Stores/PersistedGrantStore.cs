using Starshine.Abp.IdentityServer.Repositories;
using Starshine.IdentityServer.Stores;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;

namespace Starshine.Abp.IdentityServer.Stores;

/// <summary>
/// 持久授权存储
/// </summary>
public class PersistedGrantStore : IPersistedGrantStore
{
    /// <summary>
    /// 持久授权存储
    /// </summary>
    protected IPersistentGrantRepository PersistentGrantRepository { get; }

    /// <summary>
    /// 唯一标识生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }

    /// <summary>
    /// 持久授权存储
    /// </summary>
    /// <param name="persistentGrantRepository"></param>
    /// <param name="guidGenerator"></param>
    public PersistedGrantStore(IPersistentGrantRepository persistentGrantRepository,
        IGuidGenerator guidGenerator)
    {
        PersistentGrantRepository = persistentGrantRepository;
        GuidGenerator = guidGenerator;
    }

    /// <summary>
    /// 存储
    /// </summary>
    /// <param name="grant"></param>
    /// <returns></returns>
    public virtual async Task StoreAsync(Starshine.IdentityServer.Models.PersistedGrant grant)
    {
        var entity = await PersistentGrantRepository.FindByKeyAsync(grant.Key);
        if (entity == null)
        {
            entity = grant.ToPersistedGrantEntity();
            EntityHelper.TrySetId(entity, () => GuidGenerator.Create());
            await PersistentGrantRepository.InsertAsync(entity);
        }
        else
        {
            entity = grant.ToPersistedGrantEntity(entity);
            await PersistentGrantRepository.UpdateAsync(entity);
        }
    }

    /// <summary>
    /// 获取持久化授权
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual async Task<Starshine.IdentityServer.Models.PersistedGrant?> GetAsync(string key)
    {
        var persistedGrant = await PersistentGrantRepository.FindByKeyAsync(key);
        if (persistedGrant == null) return null;
        return persistedGrant.ToPersistedGrantModel();
    }

    /// <summary>
    /// 获取所有持久化授权
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<Starshine.IdentityServer.Models.PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
    {
        var persistedGrants = await PersistentGrantRepository.GetListAsync(filter.SubjectId, filter.SessionId, filter.ClientId, filter.Type);
        return persistedGrants.ConvertAll(r => r.ToPersistedGrantModel());
    }


    /// <summary>
    /// 根据key删除授权
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual async Task RemoveAsync(string key)
    {
        var persistedGrant = await PersistentGrantRepository.FindByKeyAsync(key);
        if (persistedGrant == null) return;
        await PersistentGrantRepository.DeleteAsync(persistedGrant);
    }

    /// <summary>
    /// 删除所有授权
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual async Task RemoveAllAsync(PersistedGrantFilter filter)
    {
        await PersistentGrantRepository.DeleteAsync(filter.SubjectId, filter.SessionId, filter.ClientId, filter.Type);
    }
}
