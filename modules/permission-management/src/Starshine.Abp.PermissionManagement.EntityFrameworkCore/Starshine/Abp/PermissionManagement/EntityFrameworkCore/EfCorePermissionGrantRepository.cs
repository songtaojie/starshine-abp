using Microsoft.EntityFrameworkCore;
using Starshine.Abp.Domain.Repositories.EntityFrameworkCore;
using Starshine.Abp.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.PermissionManagement.EntityFrameworkCore;

/// <summary>
/// 授权
/// </summary>
public class EfCorePermissionGrantRepository :
    EfCoreRepository<IPermissionManagementDbContext, PermissionGrant, Guid>,
    IPermissionGrantRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public EfCorePermissionGrantRepository(IDbContextProvider<IPermissionManagementDbContext> dbContextProvider,IAbpLazyServiceProvider abpLazyServiceProvider)
        : base(dbContextProvider, abpLazyServiceProvider)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<PermissionGrant?> FindAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(s =>
                s.Name == name &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey,
                GetCancellationToken(cancellationToken)
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<PermissionGrant>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="names"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<PermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                names.Contains(s.Name) &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }
}
