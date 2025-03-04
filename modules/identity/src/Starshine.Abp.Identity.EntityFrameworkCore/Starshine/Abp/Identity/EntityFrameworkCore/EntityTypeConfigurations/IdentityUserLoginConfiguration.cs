using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// IdentityUserLogin配置
    /// </summary>
    public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityUserLogin> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityUserLogin), StarshineIdentityDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(u => new { u.UserId, u.LoginProvider });
            builder.Property(u => u.LoginProvider).HasMaxLength(IdentityUserLoginConsts.MaxLoginProviderLength).IsRequired();
            builder.Property(u => u.ProviderKey).HasMaxLength(IdentityUserLoginConsts.MaxProviderKeyLength).IsRequired();
            builder.Property(u => u.ProviderDisplayName).HasMaxLength(IdentityUserLoginConsts.MaxProviderDisplayNameLength);
            builder.HasIndex(l => new { l.LoginProvider, l.ProviderKey });
            builder.ApplyObjectExtensionMappings();
        }
    }
}
