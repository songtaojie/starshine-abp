using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// ApiResource配置
    /// </summary>
    public class ApiResourceConfiguration : IEntityTypeConfiguration<ApiResource>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ApiResource> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(ApiResource), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.Property(x => x.Name).HasMaxLength(ApiResourceConsts.NameMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.DisplayName).HasMaxLength(ApiResourceConsts.DisplayNameMaxLength).IsUnicode(false);
            builder.Property(x => x.Description).HasMaxLength(ApiResourceConsts.DescriptionMaxLength).IsUnicode(false);
            builder.Property(x => x.AllowedAccessTokenSigningAlgorithms).HasMaxLength(ApiResourceConsts.AllowedAccessTokenSigningAlgorithmsMaxLength).IsUnicode(false);
            //b.HasMany(x => x.Secrets).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
            //b.HasMany(x => x.Scopes).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
            //b.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
            //b.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
