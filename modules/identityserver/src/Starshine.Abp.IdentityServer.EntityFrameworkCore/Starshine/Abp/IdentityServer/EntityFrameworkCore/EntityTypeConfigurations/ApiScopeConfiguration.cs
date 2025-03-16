using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// ApiScope配置
    /// </summary>
    public class ApiScopeConfiguration : IEntityTypeConfiguration<ApiScope>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ApiScope> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(ApiScope), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.Property(x => x.Name).HasMaxLength(ApiScopeConsts.NameMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.DisplayName).HasMaxLength(ApiScopeConsts.DisplayNameMaxLength).IsUnicode(false);
            builder.Property(x => x.Description).HasMaxLength(ApiScopeConsts.DescriptionMaxLength).IsUnicode(false);
            //builder.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiScopeId).IsRequired();
            //builder.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.ApiScopeId).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
