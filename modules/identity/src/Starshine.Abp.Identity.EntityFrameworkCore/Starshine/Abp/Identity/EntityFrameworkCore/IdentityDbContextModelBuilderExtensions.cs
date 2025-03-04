using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.Identity.EntityFrameworkCore.EntityTypeConfigurations;
using Starshine.Abp.Users.EntityFrameworkCore;
using System.Runtime.CompilerServices;
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

        if (builder.IsHostDatabase())
        {
            builder.Entity<IdentityClaimType>(b =>
            {
                b.ToTable(StarshineIdentityDbProperties.DbTablePrefix + "ClaimTypes", StarshineIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(uc => uc.Name).HasMaxLength(IdentityClaimTypeConsts.MaxNameLength)
                    .IsRequired(); // make unique
                b.Property(uc => uc.Regex).HasMaxLength(IdentityClaimTypeConsts.MaxRegexLength);
                b.Property(uc => uc.RegexDescription).HasMaxLength(IdentityClaimTypeConsts.MaxRegexDescriptionLength);
                b.Property(uc => uc.Description).HasMaxLength(IdentityClaimTypeConsts.MaxDescriptionLength);

                b.ApplyObjectExtensionMappings();
            });
        }

        builder.Entity<OrganizationUnit>(b =>
        {
            b.ToTable(StarshineIdentityDbProperties.DbTablePrefix + "OrganizationUnits", StarshineIdentityDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(ou => ou.Code).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxCodeLength)
                .HasColumnName(nameof(OrganizationUnit.Code));
            b.Property(ou => ou.DisplayName).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxDisplayNameLength)
                .HasColumnName(nameof(OrganizationUnit.DisplayName));

            b.HasMany<OrganizationUnit>().WithOne().HasForeignKey(ou => ou.ParentId);
            b.HasMany(ou => ou.Roles).WithOne().HasForeignKey(our => our.OrganizationUnitId).IsRequired();

            b.HasIndex(ou => ou.Code);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<OrganizationUnitRole>(b =>
        {
            b.ToTable(StarshineIdentityDbProperties.DbTablePrefix + "OrganizationUnitRoles", StarshineIdentityDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(ou => new { ou.OrganizationUnitId, ou.RoleId });

            b.HasOne<IdentityRole>().WithMany().HasForeignKey(ou => ou.RoleId).IsRequired();

            b.HasIndex(ou => new { ou.RoleId, ou.OrganizationUnitId });

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<IdentityUserOrganizationUnit>(b =>
        {
            b.ToTable(StarshineIdentityDbProperties.DbTablePrefix + "UserOrganizationUnits", StarshineIdentityDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(ou => new { ou.OrganizationUnitId, ou.UserId });

            b.HasOne<OrganizationUnit>().WithMany().HasForeignKey(ou => ou.OrganizationUnitId).IsRequired();

            b.HasIndex(ou => new { ou.UserId, ou.OrganizationUnitId });

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<IdentitySecurityLog>(b =>
        {
            b.ToTable(StarshineIdentityDbProperties.DbTablePrefix + "SecurityLogs", StarshineIdentityDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.TenantName).HasMaxLength(IdentitySecurityLogConsts.MaxTenantNameLength);

            b.Property(x => x.ApplicationName).HasMaxLength(IdentitySecurityLogConsts.MaxApplicationNameLength);
            b.Property(x => x.Identity).HasMaxLength(IdentitySecurityLogConsts.MaxIdentityLength);
            b.Property(x => x.Action).HasMaxLength(IdentitySecurityLogConsts.MaxActionLength);

            b.Property(x => x.UserName).HasMaxLength(IdentitySecurityLogConsts.MaxUserNameLength);

            b.Property(x => x.ClientIpAddress).HasMaxLength(IdentitySecurityLogConsts.MaxClientIpAddressLength);
            b.Property(x => x.ClientId).HasMaxLength(IdentitySecurityLogConsts.MaxClientIdLength);
            b.Property(x => x.CorrelationId).HasMaxLength(IdentitySecurityLogConsts.MaxCorrelationIdLength);
            b.Property(x => x.BrowserInfo).HasMaxLength(IdentitySecurityLogConsts.MaxBrowserInfoLength);

            b.HasIndex(x => new { x.TenantId, x.ApplicationName });
            b.HasIndex(x => new { x.TenantId, x.Identity });
            b.HasIndex(x => new { x.TenantId, x.Action });
            b.HasIndex(x => new { x.TenantId, x.UserId });

            b.ApplyObjectExtensionMappings();
        });

        if (builder.IsHostDatabase())
        {
            builder.Entity<IdentityLinkUser>(b =>
            {
                b.ToTable(StarshineIdentityDbProperties.DbTablePrefix + "LinkUsers", StarshineIdentityDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.HasIndex(x => new
                {
                    UserId = x.SourceUserId,
                    TenantId = x.SourceTenantId,
                    LinkedUserId = x.TargetUserId,
                    LinkedTenantId = x.TargetTenantId
                }).IsUnique();

                b.ApplyObjectExtensionMappings();
            });
        }

        builder.Entity<IdentityUserDelegation>(b =>
        {
            b.ToTable(StarshineIdentityDbProperties.DbTablePrefix + "UserDelegations", StarshineIdentityDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<IdentitySession>(b =>
        {
            b.ToTable(StarshineIdentityDbProperties.DbTablePrefix + "Sessions", StarshineIdentityDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.SessionId).HasMaxLength(IdentitySessionConsts.MaxSessionIdLength).IsRequired();
            b.Property(x => x.Device).HasMaxLength(IdentitySessionConsts.MaxDeviceLength).IsRequired();
            b.Property(x => x.DeviceInfo).HasMaxLength(IdentitySessionConsts.MaxDeviceInfoLength);
            b.Property(x => x.ClientId).HasMaxLength(IdentitySessionConsts.MaxClientIdLength);
            b.Property(x => x.IpAddresses).HasMaxLength(IdentitySessionConsts.MaxIpAddressesLength);

            b.HasIndex(x => x.SessionId);
            b.HasIndex(x => x.Device);
            b.HasIndex(x => new { x.TenantId, x.UserId });

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<IdentityDbContext>();
    }
}
