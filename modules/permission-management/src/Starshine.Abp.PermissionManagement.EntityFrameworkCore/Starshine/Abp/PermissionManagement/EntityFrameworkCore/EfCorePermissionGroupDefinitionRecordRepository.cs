using Starshine.Abp.Domain.Repositories.EntityFrameworkCore;
using Starshine.Abp.EntityFrameworkCore;
using Starshine.Abp.PermissionManagement.Entities;
using Starshine.Abp.PermissionManagement.Repositories;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.PermissionManagement.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
public class EfCorePermissionGroupDefinitionRecordRepository :
    EfCoreRepository<IPermissionManagementDbContext, PermissionGroupDefinitionRecord, Guid>,
    IPermissionGroupDefinitionRecordRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContextProvider"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public EfCorePermissionGroupDefinitionRecordRepository(
        IDbContextProvider<IPermissionManagementDbContext> dbContextProvider,
        IAbpLazyServiceProvider abpLazyServiceProvider)
        : base(dbContextProvider, abpLazyServiceProvider)
    {
    }
}