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
    /// IdentitySession配置
    /// </summary>
    public class IdentitySessionConfiguration : IEntityTypeConfiguration<IdentitySession>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentitySession> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentitySession), StarshineIdentityDbProperties.DbSchema);

            builder.ConfigureByConvention();
            builder.Property(x => x.SessionId).HasMaxLength(IdentitySessionConsts.MaxSessionIdLength).IsRequired();
            builder.Property(x => x.Device).HasMaxLength(IdentitySessionConsts.MaxDeviceLength).IsRequired();
            builder.Property(x => x.DeviceInfo).HasMaxLength(IdentitySessionConsts.MaxDeviceInfoLength);
            builder.Property(x => x.ClientId).HasMaxLength(IdentitySessionConsts.MaxClientIdLength);
            builder.Property(x => x.IpAddresses).HasMaxLength(IdentitySessionConsts.MaxIpAddressesLength);
            builder.HasIndex(x => x.SessionId);
            builder.HasIndex(x => x.Device);
            builder.HasIndex(x => new { x.TenantId, x.UserId });

            builder.ApplyObjectExtensionMappings();
        }
    }
}
