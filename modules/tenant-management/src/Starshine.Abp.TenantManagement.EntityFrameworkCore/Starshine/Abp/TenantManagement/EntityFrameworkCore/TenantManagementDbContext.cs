using Microsoft.EntityFrameworkCore;
using Starshine.Abp.TenantManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.TenantManagement.EntityFrameworkCore;
/// <summary>
/// 创建租户管理数据库上下文
/// </summary>
[IgnoreMultiTenancy]
[ConnectionStringName(StarshineTenantManagementDbProperties.ConnectionStringName)]
public class TenantManagementDbContext : AbpDbContext<TenantManagementDbContext>, ITenantManagementDbContext
{
    /// <summary>
    /// 租户
    /// </summary>
    public DbSet<Tenant> Tenants { get; set; }

    /// <summary>
    /// 租户连接字符串
    /// </summary>
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options"></param>
    public TenantManagementDbContext(DbContextOptions<TenantManagementDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureTenantManagement();
    }
}
