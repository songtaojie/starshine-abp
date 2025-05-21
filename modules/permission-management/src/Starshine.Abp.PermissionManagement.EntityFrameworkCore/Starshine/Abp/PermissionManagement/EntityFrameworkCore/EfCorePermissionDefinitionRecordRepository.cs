using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.Domain.Repositories.EntityFrameworkCore;
using Starshine.Abp.EntityFrameworkCore;
using Starshine.Abp.PermissionManagement.Entities;
using Starshine.Abp.PermissionManagement.Repositories;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.PermissionManagement.EntityFrameworkCore;

/// <summary>
/// 权限组定义
/// </summary>
public class EfCorePermissionDefinitionRecordRepository :
    EfCoreRepository<IPermissionManagementDbContext, PermissionDefinitionRecord, Guid>,
    IPermissionDefinitionRecordRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    public EfCorePermissionDefinitionRecordRepository(
        IDbContextProvider<IPermissionManagementDbContext> dbContextProvider,
        IAbpLazyServiceProvider abpLazyServiceProvider)
        : base(dbContextProvider, abpLazyServiceProvider)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<PermissionDefinitionRecord?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }
}