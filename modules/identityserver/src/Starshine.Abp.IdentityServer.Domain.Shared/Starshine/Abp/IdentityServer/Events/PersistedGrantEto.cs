using System;

namespace Starshine.Abp.IdentityServer.Events;

/// <summary>
/// 持续授予时间传输对象
/// </summary>
[Serializable]
public class PersistedGrantEto
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// KEY
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public required string Type { get; set; } 

    /// <summary>
    /// 主题
    /// </summary>
    public required string SubjectId { get; set; }

    /// <summary>
    /// 客户端id
    /// </summary>
    public required string ClientId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTimeOffset CreationTime { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTimeOffset? Expiration { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public string? Data { get; set; }
}
