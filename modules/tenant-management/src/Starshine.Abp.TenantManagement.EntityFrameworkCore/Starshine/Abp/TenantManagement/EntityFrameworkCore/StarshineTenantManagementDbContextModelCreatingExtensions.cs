using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.EntityFrameworkCore.Modeling;
using Starshine.Abp.TenantManagement.Entities;
using Volo.Abp;

namespace Starshine.Abp.TenantManagement.EntityFrameworkCore;

/// <summary>
/// �����⻧����ģ������ݿ������ġ�
/// </summary>
public static class StarshineTenantManagementDbContextModelCreatingExtensions
{
    /// <summary>
    /// �����⻧����ģ������ݿ������ġ�
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureTenantManagement(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        builder.Entity<Tenant>(b =>
        {
            b.ToTable(StarshineTenantManagementDbProperties.DbTablePrefix + "Tenants", StarshineTenantManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(t => t.Name).IsRequired().HasMaxLength(TenantConsts.MaxNameLength);
            b.Property(t => t.NormalizedName).IsRequired().HasMaxLength(TenantConsts.MaxNameLength);

            b.HasMany(u => u.ConnectionStrings).WithOne().HasForeignKey(uc => uc.TenantId).IsRequired();

            b.HasIndex(u => u.Name);
            b.HasIndex(u => u.NormalizedName);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<TenantConnectionString>(b =>
        {
            b.ToTable(StarshineTenantManagementDbProperties.DbTablePrefix + "TenantConnectionStrings", StarshineTenantManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.TenantId, x.Name });

            b.Property(cs => cs.Name).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxNameLength);
            b.Property(cs => cs.Value).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxValueLength);

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<TenantManagementDbContext>();
    }
}
