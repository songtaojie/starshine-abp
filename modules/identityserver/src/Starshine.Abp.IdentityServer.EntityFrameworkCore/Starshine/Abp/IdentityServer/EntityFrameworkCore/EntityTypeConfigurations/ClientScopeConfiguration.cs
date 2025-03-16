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
    /// ClientProperty配置
    /// </summary>
    public class ClientScopeConfiguration : IEntityTypeConfiguration<ClientScope>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ClientScope> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(ClientScope), IdentityServerDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.HasKey(x => new { x.ClientId, x.Scope });
            builder.Property(x => x.Scope).HasMaxLength(ClientScopeConsts.ScopeMaxLength).IsUnicode(false).IsRequired();
            builder.ApplyObjectExtensionMappings();
        }

    }
}
