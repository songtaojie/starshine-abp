using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;

namespace Starshine.Abp.TenantManagement.Entities;

/// <summary>
/// 租户
/// </summary>
public class Tenant : FullAuditedAggregateRoot<Guid>, IHasEntityVersion
{
    /// <summary>
    /// 租户名称
    /// </summary>
    public virtual string Name { get; protected set; } = default!;

    /// <summary>
    /// 规范化名称
    /// </summary>
    public virtual string? NormalizedName { get; protected set; }

    /// <summary>
    /// 实体版本
    /// </summary>
    public virtual int EntityVersion { get; protected set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    public virtual List<TenantConnectionString> ConnectionStrings { get; protected set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected Tenant()
    {
        ConnectionStrings = [];
    }

    /// <summary>
    /// 构造函数
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
    /// 获取默认连接字符串
    /// </summary>
    /// <returns></returns>
    public virtual string? FindDefaultConnectionString()
    {
        return FindConnectionString(Volo.Abp.Data.ConnectionStrings.DefaultConnectionStringName);
    }

    /// <summary>
    /// 获取连接字符串
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual string? FindConnectionString(string name)
    {
        return ConnectionStrings.FirstOrDefault(c => c.Name == name)?.Value;
    }

    /// <summary>
    /// 设置默认连接字符串
    /// </summary>
    /// <param name="connectionString"></param>
    public virtual void SetDefaultConnectionString(string connectionString)
    {
        SetConnectionString(Volo.Abp.Data.ConnectionStrings.DefaultConnectionStringName, connectionString);
    }

    /// <summary>
    /// 设置连接字符串
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
    /// 移除默认连接字符串
    /// </summary>
    public virtual void RemoveDefaultConnectionString()
    {
        RemoveConnectionString(Volo.Abp.Data.ConnectionStrings.DefaultConnectionStringName);
    }

    /// <summary>
    /// 移除连接字符串
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
    /// 设置名称
    /// </summary>
    /// <param name="name"></param>
    protected internal virtual void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TenantConsts.MaxNameLength);
    }

    /// <summary>
    /// 设置规范化名称
    /// </summary>
    /// <param name="normalizedName"></param>
    protected internal virtual void SetNormalizedName(string? normalizedName)
    {
        NormalizedName = normalizedName;
    }
}
