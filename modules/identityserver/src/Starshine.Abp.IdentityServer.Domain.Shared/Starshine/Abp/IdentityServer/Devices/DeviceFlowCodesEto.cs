using System;

namespace Starshine.Abp.IdentityServer.Devices;

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
    public string DeviceCode { get; set; } = null!;

    /// <summary>
    /// 用户代码
    /// </summary>
    public string UserCode { get; set; } = null!;

    /// <summary>
    /// 科目id
    /// </summary>
    public string SubjectId { get; set; } = null!;

    /// <summary>
    /// 会话id
    /// </summary>
    public string SessionId { get; set; } = null!;

    /// <summary>
    /// 客户端id
    /// </summary>
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime? Expiration { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public string? Data { get; set; }
}
