using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.PermissionManagement.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
[ConnectionStringName(PermissionManagementDbProperties.ConnectionStringName)]
public class PermissionManagementDbContext : AbpDbContext<PermissionManagementDbContext>, IPermissionManagementDbContext
{
    /// <summary>
    /// 
    /// </summary>
    public DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DbSet<PermissionDefinitionRecord> Permissions { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DbSet<PermissionGrant> PermissionGrants { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public PermissionManagementDbContext(DbContextOptions<PermissionManagementDbContext> options)
        : base(options)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePermissionManagement();
    }
}
