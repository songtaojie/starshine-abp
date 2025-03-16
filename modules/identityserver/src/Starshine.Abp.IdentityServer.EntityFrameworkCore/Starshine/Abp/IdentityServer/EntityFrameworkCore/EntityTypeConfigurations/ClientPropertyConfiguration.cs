using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// ClientProperty配置
    /// </summary>
    public class ClientPropertyConfiguration : IEntityTypeConfiguration<ClientProperty>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ClientProperty> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(ClientProperty), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(x => new { x.ClientId, x.Key, x.Value });
            builder.Property(x => x.Key).HasMaxLength(ClientPropertyConsts.KeyMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(ClientPropertyConsts.ValueMaxLength).IsUnicode(false).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }

    }
}
