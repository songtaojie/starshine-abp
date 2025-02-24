using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// 租户连接字符串
/// </summary>
public class TenantConnectionString : Entity
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public virtual Guid TenantId { get; protected set; }

    /// <summary>
    /// 连接字符串名称
    /// </summary>
    public virtual string Name { get; protected set; }

    /// <summary>
    /// 连接字符串值
    /// </summary>
    public virtual string Value { get; protected set; }

    /// <summary>
    /// 租户连接字符串
    /// </summary>
    protected TenantConnectionString()
    {
        Name = default!;
        Value = default!;
    }

    /// <summary>
    /// 租户连接字符串
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public TenantConnectionString(Guid tenantId, string name, string value)
    {
        TenantId = tenantId;
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConnectionStringConsts.MaxNameLength);
        Value = default!;
        SetValue(value);
    }

    /// <summary>
    /// 设置连接字符串值
    /// </summary>
    /// <param name="value"></param>
    public virtual void SetValue(string value)
    {
        Value = Check.NotNullOrWhiteSpace(value, nameof(value), TenantConnectionStringConsts.MaxValueLength);
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [TenantId, Name];
    }
}
