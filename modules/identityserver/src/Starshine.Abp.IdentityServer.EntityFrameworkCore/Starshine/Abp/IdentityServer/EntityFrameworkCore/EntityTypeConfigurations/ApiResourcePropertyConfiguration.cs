using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// ApiResourceProperty配置
    /// </summary>
    public class ApiResourcePropertyConfiguration : IEntityTypeConfiguration<ApiResourceProperty>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ApiResourceProperty> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(ApiResourceProperty), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(x => new { x.ApiResourceId, x.Key, x.Value });
            builder.Property(x => x.Key).HasMaxLength(ApiResourcePropertyConsts.KeyMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.Value).HasMaxLength(ApiResourcePropertyConsts.ValueMaxLength).IsUnicode(false).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
