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
    /// 权限名称
    /// </summary>
    public virtual string Name { get; protected set; } 

    /// <summary>
    /// 提供者名称
    /// </summary>
    public virtual string ProviderName { get; protected set; } 

    /// <summary>
    /// 提供者密钥
    /// </summary>
    public virtual string ProviderKey { get; protected internal set; } 

    /// <summary>
    /// 授权
    /// </summary>
    protected PermissionGrant()
    {
        Name = string.Empty;
        ProviderName = string.Empty;
        ProviderKey = string.Empty;
    }

    /// <summary>
    /// 授权
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="name">权限名称</param>
    /// <param name="providerName">提供者名称</param>
    /// <param name="providerKey">提供者密钥</param>
    /// <param name="tenantId">租户id</param>
    public PermissionGrant(Guid id,[NotNull] string name,[NotNull] string providerName,string providerKey, Guid? tenantId = null)
    {
        Id = id;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        ProviderName = Check.NotNullOrWhiteSpace(providerName, nameof(providerName));
        ProviderKey = providerKey;
        TenantId = tenantId;
    }
}
