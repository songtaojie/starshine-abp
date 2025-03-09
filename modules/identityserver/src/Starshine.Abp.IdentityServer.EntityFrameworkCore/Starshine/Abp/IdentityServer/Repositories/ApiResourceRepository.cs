using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// Api资源存储库
/// </summary>
public class ApiResourceRepository : EfCoreRepository<IIdentityServerDbContext, ApiResource, Guid>, IApiResourceRepository
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public ApiResourceRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
    {

    }

    /// <summary>
    /// 检查名称是否存在
    /// </summary>
    /// <param name="apiResourceName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<ApiResource?> FindByNameAsync(string apiResourceName, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(apiResource => apiResource.Id)
            .FirstOrDefaultAsync(apiResource => apiResource.Name == apiResourceName, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 通过名称查找 API 资源。
    /// </summary>
    /// <param name="apiResourceNames"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<ApiResource>> FindByNameAsync(string[] apiResourceNames, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(apiResource => apiResourceNames.Contains(apiResource.Name))
            .OrderBy(apiResource => apiResource.Name)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 通过范围名称查找 API 资源。
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<ApiResource>> GetListByScopesAsync(
        string[] scopeNames,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(api => api.Scopes.Any(x => scopeNames.Contains(x.Scope)))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 按范围获取 ApiResources 列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<ApiResource>> GetListAsync(
        string sorting,
        int skipCount,
        int maxResultCount,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter!) ||
                     !string.IsNullOrEmpty(x.Description) && x.Description.Contains(filter!) ||
                     !string.IsNullOrEmpty(x.DisplayName) && x.DisplayName.Contains(filter!))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(ApiResource.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 获取 api 资源的数量。
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<long> GetCountAsync(string? filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.Name.Contains(filter!) ||
                        !string.IsNullOrEmpty(x.Description) && x.Description.Contains(filter!) ||
                        !string.IsNullOrEmpty(x.DisplayName) && x.DisplayName.Contains(filter!))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 检查名称是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="expectedId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(ar => ar.Id != expectedId && ar.Name == name, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async override Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var resourceClaims = dbContext.Set<ApiResourceClaim>().Where(sc => sc.ApiResourceId == id);
        foreach (var scopeClaim in resourceClaims)
        {
            dbContext.Set<ApiResourceClaim>().Remove(scopeClaim);
        }

        var resourceScopes = dbContext.Set<ApiResourceScope>().Where(s => s.ApiResourceId == id);
        foreach (var scope in resourceScopes)
        {
            dbContext.Set<ApiResourceScope>().Remove(scope);
        }

        var resourceSecrets = dbContext.Set<ApiResourceSecret>().Where(s => s.ApiResourceId == id);
        foreach (var secret in resourceSecrets)
        {
            dbContext.Set<ApiResourceSecret>().Remove(secret);
        }

        var apiResourceProperties = dbContext.Set<ApiResourceProperty>().Where(s => s.ApiResourceId == id);
        foreach (var property in apiResourceProperties)
        {
            dbContext.Set<ApiResourceProperty>().Remove(property);
        }

        await base.DeleteAsync(id, autoSave, cancellationToken);
    }

    /// <summary>
    /// 附带明细
    /// </summary>
    /// <returns></returns>
    public override async Task<IQueryable<ApiResource>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
