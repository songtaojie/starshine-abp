using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// IdentityUserToken配置
    /// </summary>
    public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityUserToken> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityUserToken), StarshineIdentityDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(u => new { u.UserId, u.LoginProvider, u.Name });
            builder.Property(u => u.LoginProvider).HasMaxLength(IdentityUserTokenConsts.MaxLoginProviderLength).IsRequired();
            builder.Property(u => u.Name).HasMaxLength(IdentityUserTokenConsts.MaxNameLength).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
