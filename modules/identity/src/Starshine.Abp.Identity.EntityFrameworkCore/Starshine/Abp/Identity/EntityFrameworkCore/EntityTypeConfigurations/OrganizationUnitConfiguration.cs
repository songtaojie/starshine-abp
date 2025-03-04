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
    /// OrganizationUnit配置
    /// </summary>
    public class OrganizationUnitConfiguration : IEntityTypeConfiguration<OrganizationUnit>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<OrganizationUnit> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(OrganizationUnit), StarshineIdentityDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.Property(t => t.Code).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxCodeLength);
            builder.Property(t => t.DisplayName).IsRequired().HasMaxLength(OrganizationUnitConsts.MaxDisplayNameLength);

            builder.HasMany<OrganizationUnit>().WithOne().HasForeignKey(t => t.ParentId);
            builder.HasMany(t => t.Roles).WithOne().HasForeignKey(our => our.OrganizationUnitId).IsRequired();

            builder.HasIndex(t => t.Code);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
