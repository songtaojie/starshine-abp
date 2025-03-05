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
    /// IdentityUserOrganizationUnit配置
    /// </summary>
    public class IdentityUserOrganizationUnitConfiguration : IEntityTypeConfiguration<IdentityUserOrganizationUnit>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityUserOrganizationUnit> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityUserOrganizationUnit), StarshineIdentityDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(ou => new { ou.OrganizationUnitId, ou.UserId });
            //builder.HasOne<OrganizationUnit>().WithMany().HasForeignKey(ou => ou.OrganizationUnitId).IsRequired();
            builder.HasIndex(ou => new { ou.UserId, ou.OrganizationUnitId });
            builder.ApplyObjectExtensionMappings();
        }
    }
}
