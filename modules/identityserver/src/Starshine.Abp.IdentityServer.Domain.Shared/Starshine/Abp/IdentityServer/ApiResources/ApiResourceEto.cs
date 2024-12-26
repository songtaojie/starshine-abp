using System;
using JetBrains.Annotations;

namespace Starshine.Abp.IdentityServer.ApiResources;

/// <summary>
/// 
/// </summary>
[Serializable]
public class ApiResourceEto
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool Enabled { get; set; }
}
