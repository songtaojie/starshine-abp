using Microsoft.EntityFrameworkCore;
using Starshine.Abp.IdentityServer.Consts;
using Starshine.Abp.IdentityServer.Entities;
using Starshine.Abp.IdentityServer.EntityFrameworkCore.EntityTypeConfigurations;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore;
/// <summary>
/// IdentityServer DbContext 模型创建扩展
/// </summary>
public static class IdentityServerDbContextModelCreatingExtensions
{
    /// <summary>
    /// 模型创建扩展
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureIdentityServer(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }
        builder.ApplyConfiguration(new ClientConfiguration());
        builder.ApplyConfiguration(new ClientGrantTypeConfiguration());
        builder.ApplyConfiguration(new ClientCorsOriginConfiguration());
        builder.ApplyConfiguration(new ClientIdPRestrictionConfiguration());
        builder.ApplyConfiguration(new ClientPostLogoutRedirectUriConfiguration());
        builder.ApplyConfiguration(new ClientPropertyConfiguration());
        builder.ApplyConfiguration(new ClientRedirectUriConfiguration());
        builder.ApplyConfiguration(new ClientScopeConfiguration());
        builder.ApplyConfiguration(new ClientSecretConfiguration());
        builder.ApplyConfiguration(new ClientClaimConfiguration());

        builder.ApplyConfiguration(new IdentityResourceConfiguration());
        builder.ApplyConfiguration(new IdentityResourceClaimConfiguration());
        builder.ApplyConfiguration(new IdentityResourcePropertyConfiguration());

        
        builder.ApplyConfiguration(new ApiResourceConfiguration());
        builder.ApplyConfiguration(new ApiResourceClaimConfiguration());
        builder.ApplyConfiguration(new ApiResourcePropertyConfiguration());
        builder.ApplyConfiguration(new ApiResourceScopeConfiguration());
        builder.ApplyConfiguration(new ApiResourceSecretConfiguration());

        
        builder.ApplyConfiguration(new ApiScopeConfiguration());
        builder.ApplyConfiguration(new ApiScopeClaimConfiguration());
        builder.ApplyConfiguration(new ApiScopePropertyConfiguration());


        builder.ApplyConfiguration(new PersistedGrantConfiguration());
        builder.ApplyConfiguration(new DeviceFlowCodesConfiguration());

        builder.TryConfigureObjectExtensions<IdentityServerDbContext>();
    }
}
