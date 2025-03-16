using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// ApiScopeProperty配置
    /// </summary>
    public class ApiScopePropertyConfiguration : IEntityTypeConfiguration<ApiScopeProperty>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ApiScopeProperty> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(ApiScopeProperty), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(x => new { x.ApiScopeId, x.Key, x.Value });

            builder.Property(x => x.Key).HasMaxLength(ApiScopePropertyConsts.KeyMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(ApiScopePropertyConsts.ValueMaxLength).IsUnicode(false).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
