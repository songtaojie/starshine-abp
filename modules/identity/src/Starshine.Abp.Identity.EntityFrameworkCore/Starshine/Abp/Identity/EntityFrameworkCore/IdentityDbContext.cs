using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.Identity;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
[ConnectionStringName(StarshineIdentityDbProperties.ConnectionStringName)]
public class IdentityDbContext : AbpDbContext<IdentityDbContext>, IIdentityDbContext
{
    /// <summary>
    /// 
    /// </summary>
    public DbSet<IdentityUser> Users { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<IdentityRole> Roles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<IdentitySession> Sessions { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
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

        builder.ConfigureIdentity();
    }
}
