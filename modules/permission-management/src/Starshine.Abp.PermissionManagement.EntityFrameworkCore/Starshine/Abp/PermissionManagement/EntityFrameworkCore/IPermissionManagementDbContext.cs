using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.PermissionManagement.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
[ConnectionStringName(PermissionManagementDbProperties.ConnectionStringName)]
public interface IPermissionManagementDbContext : IEfCoreDbContext
{
    /// <summary>
    /// 
    /// </summary>
    DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; }

    /// <summary>
    /// 
    /// </summary>
    DbSet<PermissionDefinitionRecord> Permissions { get; }

    /// <summary>
    /// 
    /// </summary>
    DbSet<PermissionGrant> PermissionGrants { get; }
}
