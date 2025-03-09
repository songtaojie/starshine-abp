using JetBrains.Annotations;
using Starshine.IdentityServer;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// API��Դ
/// </summary>
public class ApiResource : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// ��Դ����
    /// </summary>
    public virtual string Name { get; protected set; }

    /// <summary>
    /// ��ʾ����
    /// </summary>
    public virtual string? DisplayName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public virtual bool Enabled { get; set; }

    /// <summary>
    /// �����������ǩ���㷨
    /// </summary>
    public virtual string? AllowedAccessTokenSigningAlgorithms { get; set; }

    /// <summary>
    /// �ڷ����ļ�����ʾ
    /// </summary>
    public virtual bool ShowInDiscoveryDocument { get; set; } = true;

    /// <summary>
    /// API��Կ
    /// </summary>
    public virtual List<ApiResourceSecret> Secrets { get; protected set; } = [];

    /// <summary>
    /// ��Χ
    /// </summary>
    public virtual List<ApiResourceScope> Scopes { get; protected set; } = [];

    /// <summary>
    /// �û�����
    /// </summary>
    public virtual List<ApiResourceClaim> UserClaims { get; protected set; } = [];

    /// <summary>
    /// ����
    /// </summary>
    public virtual List<ApiResourceProperty> Properties { get; protected set; } = [];

    /// <summary>
    /// ���캯��
    /// </summary>
    protected ApiResource()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="displayName"></param>
    /// <param name="description"></param>
    public ApiResource(Guid id, string name, string? displayName = null, string? description = null)
        : base(id)
    {
        Check.NotNull(name, nameof(name));
        Name = name;
        DisplayName = displayName;
        Description = description;
        Enabled = true;
        Scopes = [new ApiResourceScope
        {
            ApiResourceId = id,
            Scope = name
        }];
    }

    /// <summary>
    /// �����Դƾ֤
    /// </summary>
    /// <param name="value"></param>
    /// <param name="expiration"></param>
    /// <param name="type"></param>
    /// <param name="description"></param>
    public virtual void AddSecret(string value, DateTimeOffset? expiration = null, string type = IdentityServerConstants.SecretTypes.SharedSecret, string? description = null)
    {
        Secrets.Add(new ApiResourceSecret
        {
            ApiResourceId = Id,
            Value = value,
            Expiration = expiration,
            Type = type,
            Description = description
        });
    }

    /// <summary>
    /// �Ƴ���Դƾ֤
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    public virtual void RemoveSecret(string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
    {
        Secrets.RemoveAll(s => s.Value == value && s.Type == type);
    }

    /// <summary>
    /// ����API��Դƾ֤
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual ApiResourceSecret? FindSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
    {
        return Secrets.FirstOrDefault(s => s.Type == type && s.Value == value);
    }

    /// <summary>
    /// ��ӷ�Χ
    /// </summary>
    /// <param name="scope"></param>
    /// <returns></returns>
    public virtual ApiResourceScope AddScope(string scope)
    {
        var apiResourceScope = new ApiResourceScope
        {
            ApiResourceId = Id,
            Scope = scope
        };
        Scopes.Add(apiResourceScope);
        return apiResourceScope;
    }

    /// <summary>
    /// ����û�����
    /// </summary>
    /// <param name="type"></param>
    public virtual void AddUserClaim(string type)
    {
        UserClaims.Add(new ApiResourceClaim
        {
            ApiResourceId = Id,
            Type = type
        });
    }

    /// <summary>
    /// �Ƴ������û�����
    /// </summary>
    public virtual void RemoveAllUserClaims()
    {
        UserClaims.Clear();
    }

    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="type"></param>
    public virtual void RemoveClaim(string type)
    {
        UserClaims.RemoveAll(c => c.Type == type);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual ApiResourceClaim? FindClaim(string type)
    {
        return UserClaims.FirstOrDefault(c => c.Type == type);
    }

    /// <summary>
    /// �Ƴ�����ƾ֤
    /// </summary>
    public virtual void RemoveAllSecrets()
    {
        Secrets.Clear();
    }

    /// <summary>
    /// �Ƴ����з�Χ
    /// </summary>
    public virtual void RemoveAllScopes()
    {
        Scopes.Clear();
    }

    /// <summary>
    /// �Ƴ���Χ
    /// </summary>
    /// <param name="scope"></param>
    public virtual void RemoveScope(string scope)
    {
        Scopes.RemoveAll(r => r.Scope == scope);
    }

    /// <summary>
    /// ���ҷ�Χ
    /// </summary>
    /// <param name="scope"></param>
    /// <returns></returns>
    public virtual ApiResourceScope? FindScope(string scope)
    {
        return Scopes.FirstOrDefault(r => r.Scope == scope);
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public virtual void AddProperty(string key, string value)
    {
        var property = FindProperty(key);
        if (property == null)
        {
            Properties.Add(new ApiResourceProperty
            {
                ApiResourceId = Id,
                Key = key,
                Value = value
            });
        }
        else
        {
            property.Value = value;
        }
    }

    /// <summary>
    /// �Ƴ���������
    /// </summary>
    public virtual void RemoveAllProperties()
    {
        Properties.Clear();
    }

    /// <summary>
    /// �Ƴ�����
    /// </summary>
    /// <param name="key"></param>
    public virtual void RemoveProperty(string key)
    {
        Properties.RemoveAll(r => r.Key == key);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual ApiResourceProperty? FindProperty(string key)
    {
        return Properties.FirstOrDefault(r => r.Key == key);
    }
}
