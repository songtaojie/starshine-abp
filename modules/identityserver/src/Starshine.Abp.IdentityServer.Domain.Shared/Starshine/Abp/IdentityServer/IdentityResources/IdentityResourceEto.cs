using System;

namespace Starshine.Abp.IdentityServer.IdentityResources;

/// <summary>
/// 
/// </summary>
[Serializable]
public class IdentityResourceEto
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool Emphasize { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool ShowInDiscoveryDocument { get; set; }
}
