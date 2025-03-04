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
    /// IdentityUser配置
    /// </summary>
    public class IdentityRoleConfiguration: IEntityTypeConfiguration<IdentityRole>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityRole), StarshineIdentityDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.Property(u => u.Name).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNameLength);
            builder.Property(u => u.NormalizedName).IsRequired().HasMaxLength(IdentityRoleConsts.MaxNormalizedNameLength);
            builder.Property(u => u.IsDefault);
            builder.Property(u => u.IsStatic);
            builder.Property(u => u.IsPublic);
            builder.HasIndex(u => u.Name);
            builder.HasIndex(u => u.NormalizedName);
            builder.ApplyObjectExtensionMappings();
        }
    }
}
