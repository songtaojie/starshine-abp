using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.PermissionManagement.EntityFrameworkCore;

public class EfCorePermissionGroupDefinitionRecordRepository :
    EfCoreRepository<IPermissionManagementDbContext, PermissionGroupDefinitionRecord, Guid>,
    IPermissionGroupDefinitionRecordRepository
{
    public EfCorePermissionGroupDefinitionRecordRepository(
        IDbContextProvider<IPermissionManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}