using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// IdentityResourceProperty配置
    /// </summary>
    public class IdentityResourcePropertyConfiguration : IEntityTypeConfiguration<IdentityResourceProperty>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityResourceProperty> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(IdentityResourceProperty), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(x => new { x.IdentityResourceId, x.Key, x.Value });
            builder.Property(x => x.Key).HasMaxLength(IdentityResourcePropertyConsts.KeyMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(IdentityResourcePropertyConsts.ValueMaxLength).IsUnicode(false).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
