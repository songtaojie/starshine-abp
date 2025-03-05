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
    /// dentityUserDelegation配置
    /// </summary>
    public class IdentityUserDelegationConfiguration : IEntityTypeConfiguration<IdentityUserDelegation>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityUserDelegation> builder)
        {
            builder.ToTable(StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityUserDelegation), StarshineIdentityDbProperties.DbSchema);
            builder.ConfigureByConvention();
            builder.ApplyObjectExtensionMappings();
        }
    }
}
