using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// IdentityResource配置
    /// </summary>
    public class IdentityResourceConfiguration : IEntityTypeConfiguration<IdentityResource>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityResource> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(IdentityResource), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.Property(x => x.Name).HasMaxLength(IdentityResourceConsts.NameMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.DisplayName).HasMaxLength(IdentityResourceConsts.DisplayNameMaxLength).IsUnicode(false);
            builder.Property(x => x.Description).HasMaxLength(IdentityResourceConsts.DescriptionMaxLength).IsUnicode(false);
            //builder.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.IdentityResourceId).IsRequired();
            //builder.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.IdentityResourceId).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
