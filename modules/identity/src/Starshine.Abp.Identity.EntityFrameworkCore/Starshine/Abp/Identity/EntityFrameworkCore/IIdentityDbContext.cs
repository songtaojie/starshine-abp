using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.Identity;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

[ConnectionStringName(AbpIdentityDbProperties.ConnectionStringName)]
public interface IIdentityDbContext : IEfCoreDbContext
{
    DbSet<IdentityUser> Users { get; }

    DbSet<IdentityRole> Roles { get; }

    DbSet<IdentityClaimType> ClaimTypes { get; }

    DbSet<OrganizationUnit> OrganizationUnits { get; }

    DbSet<IdentitySecurityLog> SecurityLogs { get; }

    DbSet<IdentityLinkUser> LinkUsers { get; }

    DbSet<IdentityUserDelegation> UserDelegations { get; }

    DbSet<IdentitySession> Sessions { get; }
}
