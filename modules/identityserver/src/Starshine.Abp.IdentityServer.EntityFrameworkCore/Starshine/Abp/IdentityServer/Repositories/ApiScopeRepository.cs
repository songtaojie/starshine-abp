using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
/// Api 范围存储库
/// </summary>
public class ApiScopeRepository : EfCoreRepository<IIdentityServerDbContext, ApiScope, Guid>, IApiScopeRepository
{
    /// <summary>
    /// Api 范围存储库
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public ApiScopeRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }
    /// <summary>
    /// 根据名称查找Api范围
    /// </summary>
    /// <param name="scopeName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<ApiScope?> FindByNameAsync(string scopeName, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Name == scopeName, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 根据名称查找Api范围
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<ApiScope>> GetListByNameAsync(string[] scopeNames, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(scope => scopeNames.Contains(scope.Name))
            .OrderBy(scope => scope.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
    /// <summary>
    /// 获取Api范围列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<ApiScope>> GetListAsync(string sorting, int skipCount, int maxResultCount, string? filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter!) ||
                                                        !string.IsNullOrEmpty(x.Description) && x.Description.Contains(filter!) ||
                                                        !string.IsNullOrEmpty(x.DisplayName) && x.DisplayName.Contains(filter!))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(ApiScope.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 获取Api范围数量
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
    /// 检查Api范围名称是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="expectedId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.Id != expectedId && x.Name == name, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// 删除Api范围
    /// </summary>
    /// <param name="id"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = new CancellationToken())
    {
        var dbContext = await GetDbContextAsync();
        var scopeClaims = dbContext.Set<ApiScopeClaim>().Where(sc => sc.ApiScopeId == id);
        foreach (var claim in scopeClaims)
        {
            dbContext.Set<ApiScopeClaim>().Remove(claim);
        }

        var scopeProperties = dbContext.Set<ApiScopeProperty>().Where(s => s.ApiScopeId == id);
        foreach (var property in scopeProperties)
        {
            dbContext.Set<ApiScopeProperty>().Remove(property);
        }

        await base.DeleteAsync(id, autoSave, cancellationToken);
    }

    /// <summary>
    /// 获取Api范围详情
    /// </summary>
    /// <returns></returns>
    public override async Task<IQueryable<ApiScope>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
