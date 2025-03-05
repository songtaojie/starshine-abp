using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// 身份验证数据库上下文
/// </summary>
public static class IdentityDbContextModelBuilderExtensions
{
    /// <summary>
    /// 配置身份验证数据库上下文
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureIdentity([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
        
        builder.ApplyConfiguration(new IdentityUserConfiguration(builder));
        builder.ApplyConfiguration(new IdentityUserClaimConfiguration());
        builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
        builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
        builder.ApplyConfiguration(new IdentityUserTokenConfiguration());
        builder.ApplyConfiguration(new IdentityRoleConfiguration());
        builder.ApplyConfiguration(new IdentityRoleClaimConfiguration());
        builder.ApplyConfiguration(new OrganizationUnitConfiguration());
        builder.ApplyConfiguration(new OrganizationUnitRoleConfiguration());
        builder.ApplyConfiguration(new IdentityUserOrganizationUnitConfiguration());
        builder.ApplyConfiguration(new IdentitySecurityLogConfiguration());
        builder.ApplyConfiguration(new IdentityUserDelegationConfiguration());
        builder.ApplyConfiguration(new IdentitySessionConfiguration());
        
        if (builder.IsHostDatabase())
        {
            builder.ApplyConfiguration(new IdentityClaimTypeConfiguration());
            builder.ApplyConfiguration(new IdentityLinkUserConfiguration());
        }
        builder.TryConfigureObjectExtensions<IdentityDbContext>();
    }
}
