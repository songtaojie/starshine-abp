using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// PersistedGrant配置
    /// </summary>
    public class PersistedGrantConfiguration : IEntityTypeConfiguration<PersistedGrant>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<PersistedGrant> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(PersistedGrant), IdentityServerDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.Property(x => x.Key).HasMaxLength(PersistedGrantConsts.KeyMaxLength).ValueGeneratedNever();
            builder.Property(x => x.Type).HasMaxLength(PersistedGrantConsts.TypeMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.SubjectId).HasMaxLength(PersistedGrantConsts.SubjectIdMaxLength).IsUnicode(false);
            builder.Property(x => x.SessionId).HasMaxLength(PersistedGrantConsts.SessionIdMaxLength).IsUnicode(false);
            builder.Property(x => x.ClientId).HasMaxLength(PersistedGrantConsts.ClientIdMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(PersistedGrantConsts.DescriptionMaxLength).IsUnicode(false);
            builder.Property(x => x.CreationTime).IsRequired();

            builder.Property(x => x.Data).HasMaxLength(PersistedGrantConsts.DataMaxLengthValue).IsUnicode(false).IsRequired();

            builder.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });
            builder.HasIndex(x => new { x.SubjectId, x.SessionId, x.Type });
            builder.HasIndex(x => x.Expiration);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
