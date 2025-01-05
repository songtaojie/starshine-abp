using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.Identity;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
[ConnectionStringName(StarshineIdentityDbProperties.ConnectionStringName)]
public interface IIdentityDbContext : IEfCoreDbContext
{
    /// <summary>
    /// 
    /// </summary>
    DbSet<IdentityUser> Users { get; }

    /// <summary>
    /// 
    /// </summary>
    DbSet<IdentityRole> Roles { get; }

    /// <summary>
    /// 
    /// </summary>
    DbSet<IdentityClaimType> ClaimTypes { get; }

    /// <summary>
    /// 
    /// </summary>
    DbSet<OrganizationUnit> OrganizationUnits { get; }

    /// <summary>
    /// 
    /// </summary>
    DbSet<IdentitySecurityLog> SecurityLogs { get; }

    /// <summary>
    /// 
    /// </summary>
    DbSet<IdentityLinkUser> LinkUsers { get; }

    /// <summary>
    /// 
    /// </summary>
    DbSet<IdentityUserDelegation> UserDelegations { get; }

    /// <summary>
    /// 
    /// </summary>
    DbSet<IdentitySession> Sessions { get; }
}
