using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Starshine.Abp.Users.EntityFrameworkCore;
using Volo.Abp;

namespace Starshine.Abp.Identity.EntityFrameworkCore
{
    /// <summary>
    /// IdentityUser配置
    /// </summary>
    public class IdentityUserConfiguration(ModelBuilder modelBuilder) : IEntityTypeConfiguration<IdentityUser>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
           builder.ToTable( StarshineIdentityDbProperties.DbTablePrefix + nameof(IdentityUser), StarshineIdentityDbProperties.DbSchema);
           builder.ConfigureByConvention();
           builder.ConfigureStarshineUser();

           builder.Property(u => u.NormalizedUserName).IsRequired().HasMaxLength(IdentityUserConsts.MaxNormalizedUserNameLength);
           builder.Property(u => u.NormalizedEmail).IsRequired().HasMaxLength(IdentityUserConsts.MaxNormalizedEmailLength);
           builder.Property(u => u.PasswordHash).HasMaxLength(IdentityUserConsts.MaxPasswordHashLength);
           builder.Property(u => u.SecurityStamp).IsRequired().HasMaxLength(IdentityUserConsts.MaxSecurityStampLength);
           builder.Property(u => u.TwoFactorEnabled).HasDefaultValue(false);
           builder.Property(u => u.LockoutEnabled).HasDefaultValue(false);
           builder.Property(u => u.IsExternal).IsRequired().HasDefaultValue(false);
           builder.Property(u => u.AccessFailedCount).If(!modelBuilder.IsUsingOracle(), p => p.HasDefaultValue(0));

           builder.HasIndex(u => u.NormalizedUserName);
           builder.HasIndex(u => u.NormalizedEmail);
           builder.HasIndex(u => u.UserName);
           builder.HasIndex(u => u.Email);
           builder.ApplyObjectExtensionMappings();
        }
    }
}
