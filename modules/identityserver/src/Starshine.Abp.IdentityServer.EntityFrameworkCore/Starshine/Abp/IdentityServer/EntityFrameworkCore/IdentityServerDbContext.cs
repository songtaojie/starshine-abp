using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer.EntityFrameworkCore;
/// <summary>
/// 身份服务器数据库上下文
/// </summary>
[IgnoreMultiTenancy]
[ConnectionStringName(IdentityServerDbProperties.ConnectionStringName)]
public class IdentityServerDbContext : AbpDbContext<IdentityServerDbContext>, IIdentityServerDbContext
{
    #region ApiResource
    /// <summary>
    /// Api资源
    /// </summary>
    public DbSet<ApiResource> ApiResources { get; set; }

    /// <summary>
    /// Api资源属性
    /// </summary>
    public DbSet<ApiResourceSecret> ApiResourceSecrets { get; set; }

    /// <summary>
    /// Api资源声明
    /// </summary>
    public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }

    /// <summary>
    /// Api资源范围
    /// </summary>
    public DbSet<ApiResourceScope> ApiResourceScopes { get; set; }

    /// <summary>
    /// Api资源权限
    /// </summary>
    public DbSet<ApiResourceProperty> ApiResourceProperties { get; set; }

    #endregion

    #region ApiScope
    /// <summary>
    /// Api范围
    /// </summary>
    public DbSet<ApiScope> ApiScopes { get; set; }

    /// <summary>
    /// Api范围声明
    /// </summary>
    public DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }

    /// <summary>
    /// Api范围属性
    /// </summary>
    public DbSet<ApiScopeProperty> ApiScopeProperties { get; set; }

    #endregion

    #region IdentityResource
    /// <summary>
    /// 身份资源
    /// </summary>
    public DbSet<IdentityResource> IdentityResources { get; set; }

    /// <summary>
    /// 身份资源声明
    /// </summary>
    public DbSet<IdentityResourceClaim> IdentityClaims { get; set; }

    /// <summary>
    /// 身份资源属性
    /// </summary>
    public DbSet<IdentityResourceProperty> IdentityResourceProperties { get; set; }

    #endregion

    #region Client
    /// <summary>
    /// 客户端
    /// </summary>
    public DbSet<Client> Clients { get; set; }
    /// <summary>
    /// 客户端授权类型
    /// </summary>
    public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
    /// <summary>
    /// 客户端重定向地址
    /// </summary>
    public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
    /// <summary>
    /// 客户端post登出重定向地址
    /// </summary>
    public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
    /// <summary>
    /// 客户端范围
    /// </summary>
    public DbSet<ClientScope> ClientScopes { get; set; }
    /// <summary>
    /// 客户端凭证
    /// </summary>
    public DbSet<ClientSecret> ClientSecrets { get; set; }
    /// <summary>
    /// 客户端声明
    /// </summary>
    public DbSet<ClientClaim> ClientClaims { get; set; }
    /// <summary>
    /// 客户端身份提供商限制
    /// </summary>
    public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
    /// <summary>
    /// 客户端CorsOrigins
    /// </summary>
    public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
    /// <summary>
    /// 客户端属性
    /// </summary>
    public DbSet<ClientProperty> ClientProperties { get; set; }

    #endregion

    /// <summary>
    /// PersistedGrants
    /// </summary>
    public DbSet<PersistedGrant> PersistedGrants { get; set; }

    /// <summary>
    /// DeviceFlowCodes
    /// </summary>
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
    /// <summary>
    /// 身份服务器数据库上下文
    /// </summary>
    /// <param name="options"></param>
    public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options)
        : base(options)
    {

    }
    /// <summary>
    /// 配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureIdentityServer();
    }
}
