using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Starshine.Abp.PermissionManagement.EntityFrameworkCore;

/// <summary>
/// 权限管理数据库上下文
/// </summary>
[ConnectionStringName(PermissionManagementDbProperties.ConnectionStringName)]
public class PermissionManagementDbContext : AbpDbContext<PermissionManagementDbContext>, IPermissionManagementDbContext
{
    /// <summary>
    /// 权限组定义记录
    /// </summary>
    public DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; set; }

    /// <summary>
    /// 权限定义记录
    /// </summary>
    public DbSet<PermissionDefinitionRecord> Permissions { get; set; }

    /// <summary>
    /// 权限授予
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
