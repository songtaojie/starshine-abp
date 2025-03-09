using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// 客户端存储库
/// </summary>
public class ClientRepository : EfCoreRepository<IIdentityServerDbContext, Client, Guid>, IClientRepository
{
    /// <summary>
    /// 客户端存储库
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public ClientRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
    {

    }
    /// <summary>
    /// 通过客户端Id查找客户端
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<Client?> FindByClientIdAsync(
        string clientId,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.ClientId)
            .FirstOrDefaultAsync(x => x.ClientId == clientId, GetCancellationToken(cancellationToken));
    }
    /// <summary>
    /// 获取客户端列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<Client>> GetListAsync(
        string sorting, int skipCount, int maxResultCount, string? filter = null, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.ClientId.Contains(filter!))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Client.ClientName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
    /// <summary>
    /// 获取客户端数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<long> GetCountAsync(string? filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.ClientId.Contains(filter!))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
    /// <summary>
    /// 获取所有客户端允许的跨域来源
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<string>> GetAllDistinctAllowedCorsOriginsAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).ClientCorsOrigins
            .Select(x => x.Origin)
            .Distinct()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
    /// <summary>
    /// 检查客户端Id是否存在
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="expectedId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> CheckClientIdExistAsync(string clientId, Guid? expectedId = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(c => c.Id != expectedId && c.ClientId == clientId, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 删除客户端
    /// </summary>
    /// <param name="id"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async override Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        foreach (var clientGrantType in dbContext.Set<ClientGrantType>().Where(x => x.ClientId == id))
        {
            dbContext.Set<ClientGrantType>().Remove(clientGrantType);
        }

        foreach (var clientRedirectUri in dbContext.Set<ClientRedirectUri>().Where(x => x.ClientId == id))
        {
            dbContext.Set<ClientRedirectUri>().Remove(clientRedirectUri);
        }

        foreach (var clientPostLogoutRedirectUri in dbContext.Set<ClientPostLogoutRedirectUri>().Where(x => x.ClientId == id))
        {
            dbContext.Set<ClientPostLogoutRedirectUri>().Remove(clientPostLogoutRedirectUri);
        }

        foreach (var clientScope in dbContext.Set<ClientScope>().Where(x => x.ClientId == id))
        {
            dbContext.Set<ClientScope>().Remove(clientScope);
        }

        foreach (var clientSecret in dbContext.Set<ClientSecret>().Where(x => x.ClientId == id))
        {
            dbContext.Set<ClientSecret>().Remove(clientSecret);
        }

        foreach (var clientClaim in dbContext.Set<ClientClaim>().Where(x => x.ClientId == id))
        {
            dbContext.Set<ClientClaim>().Remove(clientClaim);
        }

        foreach (var clientIdPRestriction in dbContext.Set<ClientIdPRestriction>().Where(x => x.ClientId == id))
        {
            dbContext.Set<ClientIdPRestriction>().Remove(clientIdPRestriction);
        }

        foreach (var clientCorsOrigin in dbContext.Set<ClientCorsOrigin>().Where(x => x.ClientId == id))
        {
            dbContext.Set<ClientCorsOrigin>().Remove(clientCorsOrigin);
        }

        foreach (var clientProperty in dbContext.Set<ClientProperty>().Where(x => x.ClientId == id))
        {
            dbContext.Set<ClientProperty>().Remove(clientProperty);
        }

        await base.DeleteAsync(id, autoSave, cancellationToken);
    }

    /// <summary>
    /// 获取客户端详情
    /// </summary>
    /// <returns></returns>
    public override async Task<IQueryable<Client>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
