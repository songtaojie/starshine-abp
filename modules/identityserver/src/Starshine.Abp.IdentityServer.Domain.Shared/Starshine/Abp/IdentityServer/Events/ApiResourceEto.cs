using System;
using JetBrains.Annotations;

namespace Starshine.Abp.IdentityServer.Events;

/// <summary>
/// API资源Eto
/// </summary>
[Serializable]
public class ApiResourceEto
{
    /// <summary>
    /// 主键
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///名称
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
}
