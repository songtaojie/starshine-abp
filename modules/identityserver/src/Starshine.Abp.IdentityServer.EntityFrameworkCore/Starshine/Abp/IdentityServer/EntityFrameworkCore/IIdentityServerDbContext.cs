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
public interface IIdentityServerDbContext : IEfCoreDbContext
{
    #region ApiResource
    /// <summary>
    /// Api资源
    /// </summary>
    DbSet<ApiResource> ApiResources { get; }
    /// <summary>
    /// Api资源凭证
    /// </summary>
    DbSet<ApiResourceSecret> ApiResourceSecrets { get; }
    /// <summary>
    /// Api资源声明
    /// </summary>
    DbSet<ApiResourceClaim> ApiResourceClaims { get; }
    /// <summary>
    /// Api资源范围
    /// </summary>
    DbSet<ApiResourceScope> ApiResourceScopes { get; }
    /// <summary>
    /// Api资源属性
    /// </summary>
    DbSet<ApiResourceProperty> ApiResourceProperties { get; }

    #endregion

    #region ApiScope
    /// <summary>
    /// Api范围
    /// </summary>
    DbSet<ApiScope> ApiScopes { get; }
    /// <summary>
    /// Api范围声明
    /// </summary>
    DbSet<ApiScopeClaim> ApiScopeClaims { get; }
    /// <summary>
    /// Api范围属性
    /// </summary>
    DbSet<ApiScopeProperty> ApiScopeProperties { get; }

    #endregion

    #region IdentityResource
    /// <summary>
    /// 标识资源
    /// </summary>
    DbSet<IdentityResource> IdentityResources { get; }
    /// <summary>
    /// 标识资源声明
    /// </summary>
    DbSet<IdentityResourceClaim> IdentityClaims { get; }
    /// <summary>
    /// 标识资源属性
    /// </summary>
    DbSet<IdentityResourceProperty> IdentityResourceProperties { get; }

    #endregion

    #region Client
    /// <summary>
    /// 客户端
    /// </summary>
    DbSet<Client> Clients { get; }
    /// <summary>
    /// 客户端授权类型
    /// </summary>
    DbSet<ClientGrantType> ClientGrantTypes { get; }
    /// <summary>
    /// 客户端重定向地址
    /// </summary>
    DbSet<ClientRedirectUri> ClientRedirectUris { get; }
    /// <summary>
    /// 客户端注销后重定向 Uris
    /// </summary>
    DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; }
    /// <summary>
    /// 客户端权限
    /// </summary>
    DbSet<ClientScope> ClientScopes { get; }
    /// <summary>
    /// 客户端凭证
    /// </summary>
    DbSet<ClientSecret> ClientSecrets { get; }
    /// <summary>
    /// 客户端声明
    /// </summary>
    DbSet<ClientClaim> ClientClaims { get; }
    /// <summary>
    /// 客户端 IdP 限制
    /// </summary>
    DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; }
    /// <summary>
    /// 客户端 CORS 限制
    /// </summary>
    DbSet<ClientCorsOrigin> ClientCorsOrigins { get; }
    /// <summary>
    /// 客户端属性
    /// </summary>
    DbSet<ClientProperty> ClientProperties { get; }

    #endregion
    /// <summary>
    /// 持久化授权
    /// </summary>
    DbSet<PersistedGrant> PersistedGrants { get; }
    /// <summary>
    /// 设备流
    /// </summary>
    DbSet<DeviceFlowCodes> DeviceFlowCodes { get; }
}
