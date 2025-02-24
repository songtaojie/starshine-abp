using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// �⻧�����ַ���
/// </summary>
public class TenantConnectionString : Entity
{
    /// <summary>
    /// �⻧Id
    /// </summary>
    public virtual Guid TenantId { get; protected set; }

    /// <summary>
    /// �����ַ�������
    /// </summary>
    public virtual string Name { get; protected set; }

    /// <summary>
    /// �����ַ���ֵ
    /// </summary>
    public virtual string Value { get; protected set; }

    /// <summary>
    /// �⻧�����ַ���
    /// </summary>
    protected TenantConnectionString()
    {
        Name = default!;
        Value = default!;
    }

    /// <summary>
    /// �⻧�����ַ���
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
    /// ���������ַ���ֵ
    /// </summary>
    /// <param name="value"></param>
    public virtual void SetValue(string value)
    {
        Value = Check.NotNullOrWhiteSpace(value, nameof(value), TenantConnectionStringConsts.MaxValueLength);
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [TenantId, Name];
    }
}
