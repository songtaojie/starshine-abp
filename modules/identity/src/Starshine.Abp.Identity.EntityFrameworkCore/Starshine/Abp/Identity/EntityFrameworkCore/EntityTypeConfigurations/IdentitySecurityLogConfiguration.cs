using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// IdentitySecurityLog配置
    /// </summary>
    public class IdentitySecurityLogConfiguration : IEntityTypeConfiguration<IdentitySecurityLog>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentitySecurityLog> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentitySecurityLog), StarshineIdentityDbProperties.DbSchema);

            builder.ConfigureByConvention();
            builder.Property(x => x.TenantName).HasMaxLength(IdentitySecurityLogConsts.MaxTenantNameLength);
            builder.Property(x => x.ApplicationName).HasMaxLength(IdentitySecurityLogConsts.MaxApplicationNameLength);
            builder.Property(x => x.Identity).HasMaxLength(IdentitySecurityLogConsts.MaxIdentityLength);
            builder.Property(x => x.Action).HasMaxLength(IdentitySecurityLogConsts.MaxActionLength);
            builder.Property(x => x.UserName).HasMaxLength(IdentitySecurityLogConsts.MaxUserNameLength);
            builder.Property(x => x.ClientIpAddress).HasMaxLength(IdentitySecurityLogConsts.MaxClientIpAddressLength);
            builder.Property(x => x.ClientId).HasMaxLength(IdentitySecurityLogConsts.MaxClientIdLength);
            builder.Property(x => x.CorrelationId).HasMaxLength(IdentitySecurityLogConsts.MaxCorrelationIdLength);
            builder.Property(x => x.BrowserInfo).HasMaxLength(IdentitySecurityLogConsts.MaxBrowserInfoLength);

            builder.HasIndex(x => new { x.TenantId, x.ApplicationName });
            builder.HasIndex(x => new { x.TenantId, x.Identity });
            builder.HasIndex(x => new { x.TenantId, x.Action });
            builder.HasIndex(x => new { x.TenantId, x.UserId });

            builder.ApplyObjectExtensionMappings();
        }
    }
}
