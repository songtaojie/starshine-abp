using System;
using System.Collections.Generic;
using System.Linq;
using Starshine.IdentityServer;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// API资源
/// </summary>
public class ApiResource : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 资源名称
    /// </summary>
    public virtual string Name { get; protected set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public virtual string? DisplayName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public virtual bool Enabled { get; set; }

    /// <summary>
    /// 允许访问令牌签名算法
    /// </summary>
    public virtual string? AllowedAccessTokenSigningAlgorithms { get; set; }

    /// <summary>
    /// 在发现文件中显示
    /// </summary>
    public virtual bool ShowInDiscoveryDocument { get; set; } = true;

    /// <summary>
    /// API密钥
    /// </summary>
    public virtual List<ApiResourceSecret> Secrets { get; protected set; } = [];

    /// <summary>
    /// 范围
    /// </summary>
    public virtual List<ApiResourceScope> Scopes { get; protected set; } = [];

    /// <summary>
    /// 用户声明
    /// </summary>
    public virtual List<ApiResourceClaim> UserClaims { get; protected set; } = [];

    /// <summary>
    /// 属性
    /// </summary>
    public virtual List<ApiResourceProperty> Properties { get; protected set; } = [];

    /// <summary>
    /// 构造函数
    /// </summary>
    protected ApiResource()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// 构造函数
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
    /// 添加资源凭证
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
    /// 移除资源凭证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    public virtual void RemoveSecret(string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
    {
        Secrets.RemoveAll(s => s.Value == value && s.Type == type);
    }

    /// <summary>
    /// 查找API资源凭证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual ApiResourceSecret? FindSecret([NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
    {
        return Secrets.FirstOrDefault(s => s.Type == type && s.Value == value);
    }

    /// <summary>
    /// 添加范围
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
    /// 添加用户声明
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
