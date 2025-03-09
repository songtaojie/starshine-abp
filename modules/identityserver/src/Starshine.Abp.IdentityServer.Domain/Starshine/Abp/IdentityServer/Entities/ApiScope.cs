using Volo.Abp.Domain.Entities.Auditing;

namespace Starshine.Abp.IdentityServer.Entities;

/// <summary>
/// API ��Χ
/// </summary>
public class ApiScope : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public virtual bool Enabled { get; set; }

    /// <summary>
    /// ��Χ����
    /// </summary>
    public required virtual string Name { get; set; }

    /// <summary>
    /// ��Χ��ʾ����
    /// </summary>
    public virtual string? DisplayName { get; set; }

    /// <summary>
    /// ��Χ����
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// �Ƿ��ѡ
    /// </summary>
    public virtual bool Required { get; set; }

    /// <summary>
    /// �Ƿ�ǿ��
    /// </summary>
    public virtual bool Emphasize { get; set; }

    /// <summary>
    /// �Ƿ��ڷ����ĵ�����ʾ
    /// </summary>
    public virtual bool ShowInDiscoveryDocument { get; set; } = true;

    /// <summary>
    /// ��Χ����
    /// </summary>
    public virtual List<ApiScopeClaim> UserClaims { get; protected set; } = [];

    /// <summary>
    /// ��Χ����
    /// </summary>
    public virtual List<ApiScopeProperty> Properties { get; protected set; } = [];

    /// <summary>
    /// ���캯��
    /// </summary>
    public ApiScope(Guid id) : base(id)
    {

    }

    /// <summary>
    /// ��ӷ�Χ����
    /// </summary>
    /// <param name="type"></param>
    public virtual void AddUserClaim(string type)
    {
        UserClaims.Add(new ApiScopeClaim
        {
            ApiScopeId = Id,
            Type = type
        });
    }

    /// <summary>
    /// �Ƴ���Χ����
    /// </summary>
    public virtual void RemoveAllUserClaims()
    {
        UserClaims.Clear();
    }

    /// <summary>
    /// �Ƴ���Χ����
    /// </summary>
    /// <param name="type"></param>
    public virtual void RemoveClaim(string type)
    {
        UserClaims.RemoveAll(r => r.Type == type);
    }

    /// <summary>
    /// ���ҷ�Χ����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual ApiScopeClaim? FindClaim(string type)
    {
        return UserClaims.FirstOrDefault(r => r.Type == type);
    }

    /// <summary>
    /// ��ӷ�Χ����
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public virtual void AddProperty(string key, string value)
    {
        var property = FindProperty(key);
        if (property == null)
        {
            Properties.Add(new ApiScopeProperty
            {
                Key = key,
                ApiScopeId = Id,
                Value = value
            });
        }
        else
        {
            property.Value = value;
        }
    }

    /// <summary>
    /// �Ƴ����з�Χ����
    /// </summary>
    public virtual void RemoveAllProperties()
    {
        Properties.Clear();
    }

    /// <summary>
    /// �Ƴ���Χ����
    /// </summary>
    /// <param name="key"></param>
    public virtual void RemoveProperty(string key)
    {
        Properties.RemoveAll(r => r.Key == key);
    }

    /// <summary>
    /// ���ҷ�Χ����
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual ApiScopeProperty? FindProperty(string key)
    {
        return Properties.FirstOrDefault(r => r.Key == key);
    }
}
