using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Entities;
using Starshine.Abp.IdentityServer.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.IdentityServer.Repositories;
/// <summary>
///�����Դ�洢��
/// </summary>
public class IdentityResourceRepository : EfCoreRepository<IIdentityServerDbContext, IdentityResource, Guid>, IIdentityResourceRepository
{
    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public IdentityResourceRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    /// <summary>
    /// �������Ʋ��������Դ
    /// </summary>
    /// <param name="scopeNames"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityResource>> GetListByScopeNameAsync(
        string[] scopeNames,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(identityResource => scopeNames.Contains(identityResource.Name))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <returns></returns>
    public override async Task<IQueryable<IdentityResource>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    /// <summary>
    ///     
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityResource>> GetListAsync(string sorting, int skipCount, int maxResultCount,
        string? filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter!) ||
                     !string.IsNullOrEmpty(x.Description) && x.Description.Contains(filter!) ||
                     !string.IsNullOrEmpty(x.DisplayName) && x.DisplayName.Contains(filter!))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityResource.Name) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// ��ȡ����
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
    /// �������Ʋ��������Դ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentityResource?> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.Name == name, GetCancellationToken(cancellationToken));
    }

    /// <summary>
    /// ��������Ƿ����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="expectedId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(ir => ir.Id != expectedId && ir.Name == name, GetCancellationToken(cancellationToken));
    }
}
