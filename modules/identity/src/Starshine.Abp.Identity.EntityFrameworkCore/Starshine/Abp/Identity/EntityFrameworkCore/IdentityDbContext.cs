using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.Identity;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// 身份数据库上下文
/// </summary>
[ConnectionStringName(StarshineIdentityDbProperties.ConnectionStringName)]
public class IdentityDbContext : AbpDbContext<IdentityDbContext>, IIdentityDbContext
{
    /// <summary>
    /// 用户
    /// </summary>
    public DbSet<IdentityUser> Users { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    public DbSet<IdentityRole> Roles { get; set; }

    /// <summary>
    /// 声明
    /// </summary>
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }

    /// <summary>
    /// 组织
    /// </summary>
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }

    /// <summary>
    /// 安全日志
    /// </summary>
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }

    /// <summary>
    /// 关联用户
    /// </summary>
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    /// <summary>
    /// 用户委托
    /// </summary>
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    /// <summary>
    /// 会话
    /// </summary>
    public DbSet<IdentitySession> Sessions { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options"></param>
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
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
        builder.ConfigureIdentity();
    }
}
