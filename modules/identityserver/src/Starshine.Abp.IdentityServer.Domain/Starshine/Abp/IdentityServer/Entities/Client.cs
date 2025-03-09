using JetBrains.Annotations;
using Starshine.IdentityServer;
using Starshine.IdentityServer.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 客户端
/// </summary>
public class Client : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 客户端ID
    /// </summary>
    public required virtual string ClientId { get; set; }

    /// <summary>
    /// 客户端名称
    /// </summary>
    public virtual string? ClientName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 客户端URI
    /// </summary>
    public virtual string? ClientUri { get; set; }

    /// <summary>
    /// logoURI
    /// </summary>
    public virtual string? LogoUri { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public virtual bool Enabled { get; set; } = true;

    /// <summary>
    /// 协议类型
    /// </summary>
    public required virtual string ProtocolType { get; set; }

    /// <summary>
    /// 是否需要客户端密钥
    /// </summary>
    public virtual bool RequireClientSecret { get; set; }

    /// <summary>
    /// 是否需要同意
    /// </summary>
    public virtual bool RequireConsent { get; set; }

    /// <summary>
    /// 是否允许记住同意
    /// </summary>
    public virtual bool AllowRememberConsent { get; set; }

    /// <summary>
    /// 是否总是包含用户声明
    /// </summary>
    public virtual bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    /// <summary>
    /// 是否需要PKCE
    /// </summary>
    public virtual bool RequirePkce { get; set; }

    /// <summary>
    /// 是否允许明文PKCE
    /// </summary>
    public virtual bool AllowPlainTextPkce { get; set; }

    /// <summary>
    /// 是否需要请求对象
    /// </summary>
    public virtual bool RequireRequestObject { get; set; }

    /// <summary>
    /// 是否允许通过浏览器获取访问令牌
    /// </summary>
    public virtual bool AllowAccessTokensViaBrowser { get; set; }

    /// <summary>
    /// 前端注销URI
    /// </summary>
    public virtual string? FrontChannelLogoutUri { get; set; }

    /// <summary>
    /// 是否需要前端注销会话
    /// </summary>
    public virtual bool FrontChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 后端注销URI
    /// </summary>
    public virtual string? BackChannelLogoutUri { get; set; }

    /// <summary>
    /// 是否需要后端注销会话
    /// </summary>
    public virtual bool BackChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 是否允许离线访问
    /// </summary>
    public virtual bool AllowOfflineAccess { get; set; }

    /// <summary>
    /// 身份令牌生命周期
    /// </summary>
    public virtual int IdentityTokenLifetime { get; set; }

    /// <summary>
    /// 允许的签名算法
    /// </summary>
    public virtual string? AllowedIdentityTokenSigningAlgorithms { get; set; }

    /// <summary>
    /// 访问令牌生命周期
    /// </summary>
    public virtual int AccessTokenLifetime { get; set; }

    /// <summary>
    /// 授权码生命周期
    /// </summary>
    public virtual int AuthorizationCodeLifetime { get; set; }

    /// <summary>
    /// 同意生命周期
    /// </summary>
    public virtual int? ConsentLifetime { get; set; }

    /// <summary>
    /// 绝对刷新令牌生命周期
    /// </summary>
    public virtual int AbsoluteRefreshTokenLifetime { get; set; }

    /// <summary>
    /// 滑动刷新令牌生命周期
    /// </summary>
    public virtual int SlidingRefreshTokenLifetime { get; set; }

    /// <summary>
    /// 刷新令牌使用方式
    /// </summary>
    public virtual int RefreshTokenUsage { get; set; }

    /// <summary>
    /// 是否更新访问令牌声明
    /// </summary>
    public virtual bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    /// <summary>
    /// 刷新令牌过期方式
    /// </summary>
    public virtual int RefreshTokenExpiration { get; set; }

    /// <summary>
    /// 访问令牌类型
    /// </summary>
    public virtual int AccessTokenType { get; set; }

    /// <summary>
    /// 是否允许本地登录
    /// </summary>
    public virtual bool EnableLocalLogin { get; set; }

    /// <summary>
    /// 是否包含JWT
    /// </summary>
    public virtual bool IncludeJwtId { get; set; }

    /// <summary>
    /// 是否总是发送客户端声明
    /// </summary>
    public virtual bool AlwaysSendClientClaims { get; set; }

    /// <summary>
    /// 客户端声明前缀
    /// </summary>
    public virtual string? ClientClaimsPrefix { get; set; }

    /// <summary>
    /// 配对式主键
    /// </summary>
    public virtual string? PairWiseSubjectSalt { get; set; }

    /// <summary>
    /// 用户会话生命周期
    /// </summary>
    public virtual int? UserSsoLifetime { get; set; }

    /// <summary>
    /// 用户代码类型
    /// </summary>
    public virtual string? UserCodeType { get; set; }

    /// <summary>
    /// 设备代码生命周期
    /// </summary>
    public virtual int DeviceCodeLifetime { get; set; } = 300;

    /// <summary>
    /// 允许的范围
    /// </summary>
    public virtual List<ClientScope> AllowedScopes { get; set; } = [];

    /// <summary>
    /// 客户端密钥
    /// </summary>
    public virtual List<ClientSecret> ClientSecrets { get; set; } = [];

    /// <summary>
    /// 允许的授权类型
    /// </summary>
    public virtual List<ClientGrantType> AllowedGrantTypes { get; set; } = [];

    /// <summary>
    /// 允许的跨域
    /// </summary>
    public virtual List<ClientCorsOrigin> AllowedCorsOrigins { get; set; } = [];

    /// <summary>
    /// 允许的重定向URI
    /// </summary>
    public virtual List<ClientRedirectUri> RedirectUris { get; set; } = [];

    /// <summary>
    /// 允许的后重定向URI
    /// </summary>
    public virtual List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; } = [];

    /// <summary>
    /// 允许的IdP
    /// </summary>
    public virtual List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; } = [];

    /// <summary>
    /// 允许的声明
    /// </summary>
    public virtual List<ClientClaim> Claims { get; set; } = [];

    /// <summary>
    /// 允许的属性
    /// </summary>
    public virtual List<ClientProperty> Properties { get; set; } = [];

    /// <summary>
    /// 构造函数
    /// </summary>
    protected Client()
    {

    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="id"></param>
    public Client(Guid id)
    : base(id)
    {
        ProtocolType = IdentityServerConstants.ProtocolTypes.OpenIdConnect;
        RequireClientSecret = true;
        RequireConsent = false;
        AllowRememberConsent = true;
        RequirePkce = true;
        FrontChannelLogoutSessionRequired = true;
        BackChannelLogoutSessionRequired = true;
        IdentityTokenLifetime = 300;
        AccessTokenLifetime = 3600;
        AuthorizationCodeLifetime = 300;
        AbsoluteRefreshTokenLifetime = 2592000;
        SlidingRefreshTokenLifetime = 1296000;
        RefreshTokenUsage = (int)TokenUsage.OneTimeOnly;
        RefreshTokenExpiration = (int)TokenExpiration.Absolute;
        AccessTokenType = (int)Starshine.IdentityServer.Models.AccessTokenType.Jwt;
        EnableLocalLogin = true;
        ClientClaimsPrefix = "client_";
    }

    /// <summary>
    /// 添加授权类型
    /// </summary>
    /// <param name="grantType"></param>
    public virtual void AddGrantType(string grantType)
    {
        AllowedGrantTypes.Add(new ClientGrantType
        {
            ClientId = Id,
            GrantType = grantType
        });
    }

    /// <summary>
    /// 移除所有授权类型
    /// </summary>
    public virtual void RemoveAllAllowedGrantTypes()
    {
        AllowedGrantTypes.Clear();
    }

    /// <summary>
    /// 移除授权类型
    /// </summary>
    /// <param name="grantType"></param>
    public virtual void RemoveGrantType(string grantType)
    {
        AllowedGrantTypes.RemoveAll(r => r.GrantType == grantType);
    }

    /// <summary>
    ///  查找授权类型
    /// </summary>
    /// <param name="grantType"></param>
    /// <returns></returns>
    public virtual ClientGrantType? FindGrantType(string grantType)
    {
        return AllowedGrantTypes.FirstOrDefault(r => r.GrantType == grantType);
    }

    /// <summary>
    /// 添加客户端凭证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="expiration"></param>
    /// <param name="type"></param>
    /// <param name="description"></param>
    public virtual void AddSecret([NotNull] string value, DateTimeOffset? expiration = null, string type = IdentityServerConstants.SecretTypes.SharedSecret, string? description = null)
    {
        ClientSecrets.Add(new ClientSecret
        {
            ClientId = Id,
            Value = value,
            Expiration = expiration,
            Type = type,
            Description = description
        });
    }

    /// <summary>
    /// 移除客户端凭证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    public virtual void RemoveSecret(string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
    {
        ClientSecrets.RemoveAll(s => s.Value == value && s.Type == type);
    }

    /// <summary>
    /// 查找客户端凭证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual ClientSecret? FindSecret(string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
    {
        return ClientSecrets.FirstOrDefault(s => s.Type == type && s.Value == value);
    }

    /// <summary>
    /// 天津爱客户端作用域
    /// </summary>
    /// <param name="scope"></param>
    public virtual void AddScope(string scope)
    {
        AllowedScopes.Add(new ClientScope
        {
            ClientId = Id,
            Scope = scope
        });
    }

    /// <summary>
    /// 移除所有客户端作用域
    /// </summary>
    public virtual void RemoveAllScopes()
    {
        AllowedScopes.Clear();
    }

    /// <summary>
    /// 移除客户端作用域
    /// </summary>
    /// <param name="scope"></param>
    public virtual void RemoveScope(string scope)
    {
        AllowedScopes.RemoveAll(r => r.Scope == scope);
    }

    /// <summary>
    /// 查找客户端作用域
    /// </summary>
    /// <param name="scope"></param>
    /// <returns></returns>
    public virtual ClientScope? FindScope(string scope)
    {
        return AllowedScopes.FirstOrDefault(r => r.Scope == scope);
    }

    /// <summary>
    /// 添加客户端CORS
    /// </summary>
    /// <param name="origin"></param>
    public virtual void AddCorsOrigin(string origin)
    {
        AllowedCorsOrigins.Add(new ClientCorsOrigin
        {
            ClientId = Id,
            Origin = origin
        });
    }

    /// <summary>
    /// 添加客户端重定向地址
    /// </summary>
    /// <param name="redirectUri"></param>
    public virtual void AddRedirectUri(string redirectUri)
    {
        RedirectUris.Add(new ClientRedirectUri
        {
            ClientId = Id,
            RedirectUri = redirectUri
        });
    }

    /// <summary>
    /// 添加客户端注销重定向地址
    /// </summary>
    /// <param name="postLogoutRedirectUri"></param>
    public virtual void AddPostLogoutRedirectUri(string postLogoutRedirectUri)
    {
        PostLogoutRedirectUris.Add(new ClientPostLogoutRedirectUri
        {
            ClientId = Id,
            PostLogoutRedirectUri = postLogoutRedirectUri
        });
    }

    /// <summary>
    /// 移除客户端CORS
    /// </summary>
    public virtual void RemoveAllCorsOrigins()
    {
        AllowedCorsOrigins.Clear();
    }

    /// <summary>
    /// 移除客户端CORS
    /// </summary>
    /// <param name="uri"></param>
    public virtual void RemoveCorsOrigin(string uri)
    {
        AllowedCorsOrigins.RemoveAll(c => c.Origin == uri);
    }

    /// <summary>
    /// 移除客户端重定向地址
    /// </summary>
    public virtual void RemoveAllRedirectUris()
    {
        RedirectUris.Clear();
    }

    /// <summary>
    /// 移除客户端重定向地址
    /// </summary>
    /// <param name="uri"></param>
    public virtual void RemoveRedirectUri(string uri)
    {
        RedirectUris.RemoveAll(r => r.RedirectUri == uri);
    }

    /// <summary>
    /// 移除客户端注销重定向地址
    /// </summary>
    public virtual void RemoveAllPostLogoutRedirectUris()
    {
        PostLogoutRedirectUris.Clear();
    }

    /// <summary>
    /// 移除客户端注销重定向地址
    /// </summary>
    /// <param name="uri"></param>
    public virtual void RemovePostLogoutRedirectUri(string uri)
    {
        PostLogoutRedirectUris.RemoveAll(p => p.PostLogoutRedirectUri == uri);
    }

    /// <summary>
    /// 查找客户端CORS
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public virtual ClientCorsOrigin? FindCorsOrigin(string uri)
    {
        return AllowedCorsOrigins.FirstOrDefault(c => c.Origin == uri);
    }

    /// <summary>
    ///  查找客户端重定向地址
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public virtual ClientRedirectUri? FindRedirectUri(string uri)
    {
        return RedirectUris.FirstOrDefault(r => r.RedirectUri == uri);
    }

    /// <summary>
    /// 查找客户端注销重定向地址
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public virtual ClientPostLogoutRedirectUri? FindPostLogoutRedirectUri(string uri)
    {
        return PostLogoutRedirectUris.FirstOrDefault(p => p.PostLogoutRedirectUri == uri);
    }

    /// <summary>
    /// 添加客户端属性
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public virtual void AddProperty(string key, string value)
    {
        var property = FindProperty(key);
        if (property == null)
        {
            Properties.Add(new ClientProperty
            {
                ClientId = Id,
                Key = key,
                Value = value
            });
        }
        else
        {
            property.Value = value;
        }
    }

    /// <summary>
    /// 移除客户端属性
    /// </summary>
    public virtual void RemoveAllProperties()
    {
        Properties.Clear();
    }

    /// <summary>
    /// 移除客户端属性
    /// </summary>
    /// <param name="key"></param>
    public virtual void RemoveProperty(string key)
    {
        Properties.RemoveAll(c => c.Key == key);
    }

    /// <summary>
    /// 查找客户端属性
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual ClientProperty? FindProperty(string key)
    {
        return Properties.FirstOrDefault(c => c.Key == key);
    }

    /// <summary>
    /// 添加客户端声明
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public virtual void AddClaim(string type, string value)
    {
        Claims.Add(new ClientClaim
        {
            ClientId = Id,
            Type = type,
            Value = value
        });
    }

    /// <summary>
    /// 移除客户端声明
    /// </summary>
    public virtual void RemoveAllClaims()
    {
        Claims.Clear();
    }

    /// <summary>
    /// 移除客户端声明
    /// </summary>
    /// <param name="type"></param>
    public virtual void RemoveClaim(string type)
    {
        Claims.RemoveAll(c => c.Type == type);
    }

    /// <summary>
    /// 移除客户端声明
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public virtual void RemoveClaim(string type, string value)
    {
        Claims.RemoveAll(c => c.Type == type && c.Value == value);
    }

    /// <summary>
    /// 查找客户端声明
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual List<ClientClaim> FindClaims(string type)
    {
        return Claims.Where(c => c.Type == type).ToList();
    }

    /// <summary>
    /// 查找客户端声明
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual ClientClaim? FindClaim(string type, string value)
    {
        return Claims.FirstOrDefault(c => c.Type == type && c.Value == value);
    }

    /// <summary>
    /// 添加客户端身份提供商限制
    /// </summary>
    /// <param name="provider"></param>
    public virtual void AddIdentityProviderRestriction([NotNull] string provider)
    {
        IdentityProviderRestrictions.Add(new ClientIdPRestriction
        {
            ClientId = Id,
            Provider = provider
        });
    }

    /// <summary>
    /// 移除客户端身份提供商限制
    /// </summary>
    public virtual void RemoveAllIdentityProviderRestrictions()
    {
        IdentityProviderRestrictions.Clear();
    }

    /// <summary>
    /// 移除客户端身份提供商限制
    /// </summary>
    /// <param name="provider"></param>
    public virtual void RemoveIdentityProviderRestriction(string provider)
    {
        IdentityProviderRestrictions.RemoveAll(r => r.Provider == provider);
    }

    /// <summary>
    /// 查找客户端身份提供商限制
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public virtual ClientIdPRestriction? FindIdentityProviderRestriction(string provider)
    {
        return IdentityProviderRestrictions.FirstOrDefault(r => r.Provider == provider);
    }
}
