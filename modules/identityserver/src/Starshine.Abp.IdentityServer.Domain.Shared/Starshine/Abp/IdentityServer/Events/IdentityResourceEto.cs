using System;

namespace Starshine.Abp.IdentityServer.Events;

/// <summary>
/// 身份资源ETO
/// </summary>
[Serializable]
public class IdentityResourceEto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 资源名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 是否必须
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// 强调
    /// </summary>
    public bool Emphasize { get; set; }

    /// <summary>
    /// 发现文档显示
    /// </summary>
    public bool ShowInDiscoveryDocument { get; set; }
}
