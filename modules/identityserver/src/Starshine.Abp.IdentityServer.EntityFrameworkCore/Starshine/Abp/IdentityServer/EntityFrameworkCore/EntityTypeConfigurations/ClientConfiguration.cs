using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Starshine.Abp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starshine.Abp.IdentityServer.Consts;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations
{
    /// <summary>
    /// Client配置
    /// </summary>
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable(IdentityServerDbProperties.DbTablePrefix + nameof(Client), IdentityServerDbProperties.DbSchema);

            builder.ConfigureByConvention();

            builder.Property(x => x.ClientId).HasMaxLength(ClientConsts.ClientIdMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.ProtocolType).HasMaxLength(ClientConsts.ProtocolTypeMaxLength).IsUnicode(false).IsRequired();
            builder.Property(x => x.ClientName).HasMaxLength(ClientConsts.ClientNameMaxLength).IsUnicode(false);
            builder.Property(x => x.ClientUri).HasMaxLength(ClientConsts.ClientUriMaxLength).IsUnicode(false);
            builder.Property(x => x.LogoUri).HasMaxLength(ClientConsts.LogoUriMaxLength).IsUnicode(false);
            builder.Property(x => x.Description).HasMaxLength(ClientConsts.DescriptionMaxLength).IsUnicode(false);
            builder.Property(x => x.FrontChannelLogoutUri).HasMaxLength(ClientConsts.FrontChannelLogoutUriMaxLength).IsUnicode(false);
            builder.Property(x => x.BackChannelLogoutUri).HasMaxLength(ClientConsts.BackChannelLogoutUriMaxLength).IsUnicode(false);
            builder.Property(x => x.ClientClaimsPrefix).HasMaxLength(ClientConsts.ClientClaimsPrefixMaxLength).IsUnicode(false);
            builder.Property(x => x.PairWiseSubjectSalt).HasMaxLength(ClientConsts.PairWiseSubjectSaltMaxLength).IsUnicode(false);
            builder.Property(x => x.UserCodeType).HasMaxLength(ClientConsts.UserCodeTypeMaxLength).IsUnicode(false);
            builder.Property(x => x.AllowedIdentityTokenSigningAlgorithms).HasMaxLength(ClientConsts.AllowedIdentityTokenSigningAlgorithms).IsUnicode(false);

            //builder.HasMany(x => x.AllowedScopes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
            //builder.HasMany(x => x.ClientSecrets).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
            //builder.HasMany(x => x.AllowedGrantTypes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
            //builder.HasMany(x => x.AllowedCorsOrigins).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
            //builder.HasMany(x => x.RedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
            //builder.HasMany(x => x.PostLogoutRedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
            //builder.HasMany(x => x.IdentityProviderRestrictions).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
            //builder.HasMany(x => x.Claims).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
            //builder.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.ClientId).IsRequired();

            builder.HasIndex(x => x.ClientId);

            builder.ApplyObjectExtensionMappings();
        }
    }
}
