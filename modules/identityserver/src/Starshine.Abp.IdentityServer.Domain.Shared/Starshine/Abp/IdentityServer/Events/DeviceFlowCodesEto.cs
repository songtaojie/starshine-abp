using System;

namespace Starshine.Abp.IdentityServer.Events;

/// <summary>
/// 设备流代码
/// </summary>
[Serializable]
public class DeviceFlowCodesEto
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 设备编码
    /// </summary>
    public required string DeviceCode { get; set; }

    /// <summary>
    /// 用户代码
    /// </summary>
    public required string UserCode { get; set; }

    /// <summary>
    /// 科目id
    /// </summary>
    public required string SubjectId { get; set; }

    /// <summary>
    /// 会话id
    /// </summary>
    public required string SessionId { get; set; }

    /// <summary>
    /// 客户端id
    /// </summary>
    public required string ClientId { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTimeOffset? Expiration { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public string? Data { get; set; }
}
