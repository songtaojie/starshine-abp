using System;

namespace Starshine.Abp.IdentityServer.Grants;

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
    /// 
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string SubjectId { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? Expiration { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Data { get; set; }
}
