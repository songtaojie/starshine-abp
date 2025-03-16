using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// DeviceFlowCodes配置
    /// </summary>
    public class DeviceFlowCodesConfiguration : IEntityTypeConfiguration<DeviceFlowCodes>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<DeviceFlowCodes> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(DeviceFlowCodes), IdentityServerDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.Property(x => x.DeviceCode).HasMaxLength(DeviceFlowCodesConsts.DeviceCodeMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.UserCode).HasMaxLength(DeviceFlowCodesConsts.UserCodeMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.SubjectId).HasMaxLength(DeviceFlowCodesConsts.SubjectIdMaxLength).IsUnicode(false);
            builder.Property(x => x.SessionId).HasMaxLength(DeviceFlowCodesConsts.SessionIdMaxLength).IsUnicode(false);
            builder.Property(x => x.ClientId).HasMaxLength(DeviceFlowCodesConsts.ClientIdMaxLength).IsRequired().IsUnicode(false);
            builder.Property(x => x.Description).HasMaxLength(DeviceFlowCodesConsts.DescriptionMaxLength).IsUnicode(false);
            builder.Property(x => x.CreationTime).IsRequired();
            builder.Property(x => x.Expiration).IsRequired();

            builder.Property(x => x.Data).HasMaxLength(DeviceFlowCodesConsts.DataMaxLength).IsUnicode(false).IsRequired();

            builder.HasIndex(x => new { x.UserCode });
            builder.HasIndex(x => x.DeviceCode).IsUnique();
            builder.HasIndex(x => x.Expiration);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
