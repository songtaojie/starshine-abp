﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// 持久授权存储库
/// </summary>
public class PersistentGrantRepository : EfCoreRepository<IIdentityServerDbContext, PersistedGrant, Guid>, IPersistentGrantRepository
{
    /// <summary>
    /// 持久授权存储库
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public PersistentGrantRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    /// <summary>
    ///  根据条件查找授权
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="sessionId"></param>
    /// <param name="clientId"></param>
    /// <param name="type"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<PersistedGrant>> GetListAsync(string? subjectId, string? sessionId, string? clientId, string? type, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await FilterAsync(subjectId, sessionId, clientId, type))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 根据key查找授权
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<PersistedGrant?> FindByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Key == key, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 根据subjectId查找授权
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<PersistedGrant>> GetListBySubjectIdAsync(string subjectId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.SubjectId == subjectId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 根据过期时间查找授权
    /// </summary>
    /// <param name="maxExpirationDate"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<PersistedGrant>> GetListByExpirationAsync(DateTimeOffset maxExpirationDate, int maxResultCount, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Expiration != null && x.Expiration < maxExpirationDate)
            .OrderBy(x => x.ClientId)
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 根据过期时间删除授权
    /// </summary>
    /// <param name="maxExpirationDate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteExpirationAsync(DateTimeOffset maxExpirationDate, CancellationToken cancellationToken = default)
    {
        await DeleteDirectAsync(x => x.Expiration != null && x.Expiration < maxExpirationDate, cancellationToken);
    }

    /// <summary>
    /// 删除授权
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="sessionId"></param>
    /// <param name="clientId"></param>
    /// <param name="type"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(string? subjectId = null, string? sessionId = null,
        string? clientId = null,
        string? type = null,
        CancellationToken cancellationToken = default)
    {
        var persistedGrants = await (await FilterAsync(subjectId, sessionId, clientId, type)).ToListAsync(GetCancellationToken(cancellationToken));

        var dbSet = await GetDbSetAsync();

        foreach (var persistedGrant in persistedGrants)
        {
            dbSet.Remove(persistedGrant);
        }
    }

    private async Task<IQueryable<PersistedGrant>> FilterAsync(string? subjectId, string? sessionId,
        string? clientId,
        string? type)
    {
        return (await GetDbSetAsync())
            .WhereIf(!subjectId.IsNullOrWhiteSpace(), x => x.SubjectId == subjectId)
            .WhereIf(!sessionId.IsNullOrWhiteSpace(), x => x.SessionId == sessionId)
            .WhereIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .WhereIf(!type.IsNullOrWhiteSpace(), x => x.Type == type);
    }
}
