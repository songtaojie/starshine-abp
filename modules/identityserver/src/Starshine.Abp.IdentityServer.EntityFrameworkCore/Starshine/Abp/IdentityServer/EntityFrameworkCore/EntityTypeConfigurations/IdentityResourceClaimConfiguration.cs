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
    public class IdentityResourceClaimConfiguration : IEntityTypeConfiguration<IdentityResourceClaim>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityResourceClaim> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(IdentityResourceClaim), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(x => new { x.IdentityResourceId, x.Type });
            builder.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsUnicode(false).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
