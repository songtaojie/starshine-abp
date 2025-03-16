using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// ClientCorsOrigin配置
    /// </summary>
    public class ClientCorsOriginConfiguration : IEntityTypeConfiguration<ClientCorsOrigin>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ClientCorsOrigin> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(ClientCorsOrigin), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(x => new { x.ClientId, x.Origin });
            builder.Property(x => x.Origin).HasMaxLength(ClientCorsOriginConsts.OriginMaxLength).IsUnicode(false).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
