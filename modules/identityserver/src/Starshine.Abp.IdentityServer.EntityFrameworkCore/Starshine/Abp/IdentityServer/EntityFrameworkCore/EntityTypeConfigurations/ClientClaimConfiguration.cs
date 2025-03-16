using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// ClientClaim配置
    /// </summary>
    public class ClientClaimConfiguration : IEntityTypeConfiguration<ClientClaim>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ClientClaim> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(ClientClaim), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(x => new { x.ClientId, x.Type, x.Value });
            builder.Property(x => x.Type).HasMaxLength(ClientClaimConsts.TypeMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(ClientClaimConsts.ValueMaxLength).IsUnicode(false).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
