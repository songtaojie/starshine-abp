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
    /// OrganizationUnitRole配置
    /// </summary>
    public class OrganizationUnitRoleConfiguration : IEntityTypeConfiguration<OrganizationUnitRole>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<OrganizationUnitRole> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(OrganizationUnitRole), StarshineIdentityDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(ou => new { ou.OrganizationUnitId, ou.RoleId });
            //builder.HasOne<IdentityRole>().WithMany().HasForeignKey(ou => ou.RoleId).IsRequired();
            builder.HasIndex(ou => new { ou.RoleId, ou.OrganizationUnitId });
            builder.ApplyObjectExtensionMappings();
        }
    }
}
