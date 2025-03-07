using System;

namespace Starshine.Abp.IdentityServer.Events;

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
    /// 客户端id
    /// </summary>
    public required string ClientId { get; set; }

    /// <summary>
    /// 客户端名称
    /// </summary>
    public required string ClientName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 客户端uri
    /// </summary>
    public string? ClientUri { get; set; }

    /// <summary>
    /// Logo uri
    /// </summary>
    public string? LogoUri { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 协议类型
    /// </summary>
    public string? ProtocolType { get; set; }

    /// <summary>
    /// 需要客户端机密
    /// </summary>
    public bool RequireClientSecret { get; set; }

    /// <summary>
    /// 需要同意
    /// </summary>
    public bool RequireConsent { get; set; }

    /// <summary>
    /// 允许记住同意
    /// </summary>
    public bool AllowRememberConsent { get; set; }

    /// <summary>
    /// 始终包含 UserClaimsInIdToken
    /// </summary>
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    /// <summary>
    /// 需要 Pkce
    /// </summary>
    public bool RequirePkce { get; set; }

    /// <summary>
    /// 允许纯文本协议
    /// </summary>
    public bool AllowPlainTextPkce { get; set; }

    /// <summary>
    /// 允许通过浏览器访问令牌
    /// </summary>
    public bool AllowAccessTokensViaBrowser { get; set; }

    /// <summary>
    /// 前通道注销 Uri
    /// </summary>
    public string? FrontChannelLogoutUri { get; set; }

    /// <summary>
    /// 需要前置通道注销会话
    /// </summary>
    public bool FrontChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 后通道注销 Uri
    /// </summary>
    public string BackChannelLogoutUri { get; set; } = null!;

    /// <summary>
    /// 返回频道注销会话必需
    /// </summary>
    public bool BackChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 允许离线访问
    /// </summary>
    public bool AllowOfflineAccess { get; set; }

    /// <summary>
    /// 身份令牌生命周期
    /// </summary>
    public int IdentityTokenLifetime { get; set; }

    /// <summary>
    /// 访问令牌生命周期
    /// </summary>
    public int AccessTokenLifetime { get; set; }

    /// <summary>
    /// 授权码有效期
    /// </summary>
    public int AuthorizationCodeLifetime { get; set; }

    /// <summary>
    /// 同意有效期
    /// </summary>
    public int? ConsentLifetime { get; set; }

    /// <summary>
    /// 绝对刷新令牌生命周期
    /// </summary>
    public int AbsoluteRefreshTokenLifetime { get; set; }

    /// <summary>
    /// 滑动刷新令牌生命周期
    /// </summary>
    public int SlidingRefreshTokenLifetime { get; set; }

    /// <summary>
    /// RefreshToken 使用情况
    /// </summary>
    public int RefreshTokenUsage { get; set; }

    /// <summary>
    /// 刷新时更新访问令牌声明
    /// </summary>
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    /// <summary>
    /// 刷新令牌过期
    /// </summary>
    public int RefreshTokenExpiration { get; set; }

    /// <summary>
    /// 访问令牌类型
    /// </summary>
    public int AccessTokenType { get; set; }

    /// <summary>
    /// 启用本地登录
    /// </summary>
    public bool EnableLocalLogin { get; set; }

    /// <summary>
    /// 包含 JwtId
    /// </summary>
    public bool IncludeJwtId { get; set; }

    /// <summary>
    /// 总是发送客户端声明
    /// </summary>
    public bool AlwaysSendClientClaims { get; set; }

    /// <summary>
    /// 客户端声明前缀
    /// </summary>
    public string? ClientClaimsPrefix { get; set; }

    /// <summary>
    /// 成对主题盐
    /// </summary>
    public string? PairWiseSubjectSalt { get; set; }

    /// <summary>
    /// 用户 Sso 生命周期
    /// </summary>
    public int? UserSsoLifetime { get; set; }

    /// <summary>
    /// 用户代码类型
    /// </summary>
    public string? UserCodeType { get; set; }

    /// <summary>
    /// 设备代码生命周期
    /// </summary>
    public int DeviceCodeLifetime { get; set; }
}
