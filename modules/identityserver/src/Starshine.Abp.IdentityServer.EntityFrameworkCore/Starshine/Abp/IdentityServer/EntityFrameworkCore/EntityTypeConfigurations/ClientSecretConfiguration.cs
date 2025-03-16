using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// ClientSecret配置
    /// </summary>
    public class ClientSecretConfiguration : IEntityTypeConfiguration<ClientSecret>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ClientSecret> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(ClientSecret), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(x => new { x.ClientId, x.Type, x.Value });
            builder.Property(x => x.Type).HasMaxLength(ClientSecretConsts.TypeMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(ClientSecretConsts.ValueMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(ClientSecretConsts.DescriptionMaxLength).IsUnicode(false);
            builder.ApplyObjectExtensionMappings();
        }
    }
}
