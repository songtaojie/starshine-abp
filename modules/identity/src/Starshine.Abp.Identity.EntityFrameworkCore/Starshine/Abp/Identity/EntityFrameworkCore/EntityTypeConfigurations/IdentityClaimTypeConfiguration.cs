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
    /// IdentityClaimType配置
    /// </summary>
    public class IdentityClaimTypeConfiguration : IEntityTypeConfiguration<IdentityClaimType>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityClaimType> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityClaimType), StarshineIdentityDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.Property(t => t.Name).HasMaxLength(IdentityClaimTypeConsts.MaxNameLength)
                .IsRequired(); // make unique
            builder.Property(t => t.Regex).HasMaxLength(IdentityClaimTypeConsts.MaxRegexLength);
            builder.Property(t => t.RegexDescription).HasMaxLength(IdentityClaimTypeConsts.MaxRegexDescriptionLength);
            builder.Property(t => t.Description).HasMaxLength(IdentityClaimTypeConsts.MaxDescriptionLength);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
