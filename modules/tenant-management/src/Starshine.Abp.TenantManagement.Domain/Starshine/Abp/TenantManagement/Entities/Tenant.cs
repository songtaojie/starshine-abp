using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace Starshine.Abp.TenantManagement.Entities;

/// <summary>
/// �⻧
/// </summary>
public class Tenant : FullAuditedAggregateRoot<Guid>, IHasEntityVersion
{
    /// <summary>
    /// �⻧����
    /// </summary>
    public virtual string Name { get; protected set; } = default!;

    /// <summary>
    /// �淶������
    /// </summary>
    public virtual string? NormalizedName { get; protected set; }

    /// <summary>
    /// ʵ��汾
    /// </summary>
    public virtual int EntityVersion { get; protected set; }

    /// <summary>
    /// �����ַ���
    /// </summary>
    public virtual List<TenantConnectionString> ConnectionStrings { get; protected set; }

    /// <summary>
    /// ���캯��
    /// </summary>
    protected Tenant()
    {
        ConnectionStrings = [];
    }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="normalizedName"></param>
    protected internal Tenant(Guid id, [NotNull] string name, string? normalizedName)
        : base(id)
    {
        SetName(name);
        SetNormalizedName(normalizedName);
        ConnectionStrings = [];
    }

    /// <summary>
    /// ��ȡĬ�������ַ���
    /// </summary>
    /// <returns></returns>
    public virtual string? FindDefaultConnectionString()
    {
        return FindConnectionString(Volo.Abp.Data.ConnectionStrings.DefaultConnectionStringName);
    }

    /// <summary>
    /// ��ȡ�����ַ���
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual string? FindConnectionString(string name)
    {
        return ConnectionStrings.FirstOrDefault(c => c.Name == name)?.Value;
    }

    /// <summary>
    /// ����Ĭ�������ַ���
    /// </summary>
    /// <param name="connectionString"></param>
    public virtual void SetDefaultConnectionString(string connectionString)
    {
        SetConnectionString(Volo.Abp.Data.ConnectionStrings.DefaultConnectionStringName, connectionString);
    }

    /// <summary>
    /// ���������ַ���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="connectionString"></param>
    public virtual void SetConnectionString(string name, string connectionString)
    {
        var tenantConnectionString = ConnectionStrings.FirstOrDefault(x => x.Name == name);

        if (tenantConnectionString != null)
        {
            tenantConnectionString.SetValue(connectionString);
        }
        else
        {
            ConnectionStrings.Add(new TenantConnectionString(Id, name, connectionString));
        }
    }

    /// <summary>
    /// �Ƴ�Ĭ�������ַ���
    /// </summary>
    public virtual void RemoveDefaultConnectionString()
    {
        RemoveConnectionString(Volo.Abp.Data.ConnectionStrings.DefaultConnectionStringName);
    }

    /// <summary>
    /// �Ƴ������ַ���
    /// </summary>
    /// <param name="name"></param>
    public virtual void RemoveConnectionString(string name)
    {
        var tenantConnectionString = ConnectionStrings.FirstOrDefault(x => x.Name == name);

        if (tenantConnectionString != null)
        {
            ConnectionStrings.Remove(tenantConnectionString);
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="name"></param>
    protected internal virtual void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConsts.MaxNameLength);
    }

    /// <summary>
    /// ���ù淶������
    /// </summary>
    /// <param name="normalizedName"></param>
    protected internal virtual void SetNormalizedName(string? normalizedName)
    {
        NormalizedName = normalizedName;
    }
}
