using Microsoft.EntityFrameworkCore;
using Starshine.Abp.EntityFrameworkCore;
using Starshine.Abp.TenantManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.TenantManagement.EntityFrameworkCore;

/// <summary>
/// TenantManagementDbContext
/// </summary>
[IgnoreMultiTenancy]
[ConnectionStringName(StarshineTenantManagementDbProperties.ConnectionStringName)]
public interface ITenantManagementDbContext : IEfCoreDbContext
{
    /// <summary>
    /// Tenants
    /// </summary>
    DbSet<Tenant> Tenants { get; }

    /// <summary>
    /// TenantConnectionStrings
    /// </summary>
    DbSet<TenantConnectionString> TenantConnectionStrings { get; }
}
