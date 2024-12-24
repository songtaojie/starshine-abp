using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 授权
/// </summary>
public class PermissionGrant : BasicAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 租户
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    public virtual string Name { get; protected set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    public virtual string ProviderName { get; protected set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public virtual string? ProviderKey { get; protected internal set; }

    /// <summary>
    /// 
    /// </summary>
    protected PermissionGrant()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="tenantId"></param>
    public PermissionGrant(
        Guid id,
        [NotNull] string name,
        [NotNull] string providerName,
        string? providerKey,
        Guid? tenantId = null)
    {
        Id = id;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        ProviderName = Check.NotNullOrWhiteSpace(providerName, nameof(providerName));
        ProviderKey = providerKey;
        TenantId = tenantId;
    }
}
