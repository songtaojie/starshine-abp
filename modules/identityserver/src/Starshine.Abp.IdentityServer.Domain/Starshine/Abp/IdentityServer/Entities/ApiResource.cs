using System;
using System.Collections.Generic;
using System.Linq;
using Starshine.IdentityServer;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;

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
        Scopes = [new ApiResourceScope(id, name)];
    }

    /// <summary>
    /// �����Դƾ֤
    /// </summary>
    /// <param name="value"></param>
    /// <param name="expiration"></param>
    /// <param name="type"></param>
    /// <param name="description"></param>
    public virtual void AddSecret(string value, DateTimeOffset? expiration = null,string type = IdentityServerConstants.SecretTypes.SharedSecret,string? description = null)
    {
        Secrets.Add(new ApiResourceSecret(Id, value, expiration, type, description));
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
        var apiResourceScope = new ApiResourceScope(Id, scope);
        Scopes.Add(apiResourceScope);
        return apiResourceScope;
    }

    /// <summary>
    /// ����û�����
    /// </summary>
    /// <param name="type"></param>
    public virtual void AddUserClaim([NotNull] string type)
    {
        UserClaims.Add(new ApiResourceClaim(Id, type));
    }

    public virtual void RemoveAllUserClaims()
    {
        UserClaims.Clear();
    }

    public virtual void RemoveClaim(string type)
    {
        UserClaims.RemoveAll(c => c.Type == type);
    }

    public virtual ApiResourceClaim? FindClaim(string type)
    {
        return UserClaims.FirstOrDefault(c => c.Type == type);
    }

    public virtual void RemoveAllSecrets()
    {
        Secrets.Clear();
    }

    public virtual void RemoveAllScopes()
    {
        Scopes.Clear();
    }

    public virtual void RemoveScope(string scope)
    {
        Scopes.RemoveAll(r => r.Scope == scope);
    }

    public virtual ApiResourceScope? FindScope(string scope)
    {
        return Scopes.FirstOrDefault(r => r.Scope == scope);
    }

    public virtual void AddProperty([NotNull] string key, string value)
    {
        var property = FindProperty(key);
        if (property == null)
        {
            Properties.Add(new ApiResourceProperty(Id, key, value));
        }
        else
        {
            property.Value = value;
        }
    }

    public virtual void RemoveAllProperties()
    {
        Properties.Clear();
    }

    public virtual void RemoveProperty(string key)
    {
        Properties.RemoveAll(r => r.Key == key);
    }

    public virtual ApiResourceProperty? FindProperty(string key)
    {
        return Properties.FirstOrDefault(r => r.Key == key);
    }
}
