using System;

namespace Starshine.Abp.IdentityServer.Clients;

/// <summary>
/// 客户端事件传输对象
/// </summary>
[Serializable]
public class ClientEto
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string ClientName { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string ClientUri { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string LogoUri { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public string ProtocolType { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public bool RequireClientSecret { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool RequireConsent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowRememberConsent { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool RequirePkce { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowPlainTextPkce { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowAccessTokensViaBrowser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string FrontChannelLogoutUri { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public bool FrontChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string BackChannelLogoutUri { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public bool BackChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AllowOfflineAccess { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int IdentityTokenLifetime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int AccessTokenLifetime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int AuthorizationCodeLifetime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? ConsentLifetime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int AbsoluteRefreshTokenLifetime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int SlidingRefreshTokenLifetime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int RefreshTokenUsage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int RefreshTokenExpiration { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int AccessTokenType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool EnableLocalLogin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IncludeJwtId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool AlwaysSendClientClaims { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ClientClaimsPrefix { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string PairWiseSubjectSalt { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public int? UserSsoLifetime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string UserCodeType { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public int DeviceCodeLifetime { get; set; }
}
