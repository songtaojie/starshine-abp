using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// IdentityUserRole配置
    /// </summary>
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityUserRole> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityUserRole), StarshineIdentityDbProperties.DbSchema);

            builder.ConfigureByConvention();
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasIndex(ur => new { ur.RoleId, ur.UserId });
            builder.ApplyObjectExtensionMappings();
        }
    }
}
