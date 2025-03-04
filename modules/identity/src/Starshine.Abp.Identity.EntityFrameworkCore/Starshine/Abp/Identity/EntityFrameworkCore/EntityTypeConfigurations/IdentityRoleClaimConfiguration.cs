using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// IdentityRoleClaim配置
    /// </summary>
    public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityRoleClaim> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityRoleClaim), StarshineIdentityDbProperties.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(t => t.Id).ValueGeneratedNever();
            builder.Property(t => t.ClaimType).HasMaxLength(IdentityRoleClaimConsts.MaxClaimTypeLength).IsRequired();
            builder.Property(t => t.ClaimValue).HasMaxLength(IdentityRoleClaimConsts.MaxClaimValueLength);
            builder.HasIndex(t => t.RoleId);
            builder.ApplyObjectExtensionMappings();
        }
    }
}
