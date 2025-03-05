using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// IdentityLinkUser配置
    /// </summary>
    public class IdentityLinkUserConfiguration : IEntityTypeConfiguration<IdentityLinkUser>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityLinkUser> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityLinkUser), StarshineIdentityDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasIndex(x => new
            {
                UserId = x.SourceUserId,
                TenantId = x.SourceTenantId,
                LinkedUserId = x.TargetUserId,
                LinkedTenantId = x.TargetTenantId
            }).IsUnique();

            builder.ApplyObjectExtensionMappings();
        }
    }
}
