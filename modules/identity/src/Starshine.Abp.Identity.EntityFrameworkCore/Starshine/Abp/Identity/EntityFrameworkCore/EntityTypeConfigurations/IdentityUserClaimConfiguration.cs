using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// IdentityUserClaim配置
    /// </summary>
    public class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityUserClaim> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityUserClaim), StarshineIdentityDbProperties.DbSchema);

            builder.ConfigureByConvention();
            builder.Property(t => t.Id).ValueGeneratedNever();
            builder.Property(t => t.ClaimType).HasMaxLength(IdentityUserClaimConsts.MaxClaimTypeLength).IsRequired();
            builder.Property(t => t.ClaimValue).HasMaxLength(IdentityUserClaimConsts.MaxClaimValueLength);
            builder.HasIndex(t => t.UserId);
            builder.ApplyObjectExtensionMappings();
        }
    }
}
