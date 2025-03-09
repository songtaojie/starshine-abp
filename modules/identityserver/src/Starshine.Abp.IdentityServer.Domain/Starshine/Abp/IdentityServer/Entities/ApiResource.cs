using JetBrains.Annotations;
using Starshine.IdentityServer;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

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
        Scopes = [new ApiResourceScope
        {
            ApiResourceId = id,
            Scope = name
        }];
    }

    /// <summary>
    /// 添加资源凭证
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
        var apiResourceScope = new ApiResourceScope
        {
            ApiResourceId = Id,
            Scope = scope
        };
        Scopes.Add(apiResourceScope);
        return apiResourceScope;
    }

    /// <summary>
    /// 添加用户声明
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
    /// 移除所有用户声明
    /// </summary>
    public virtual void RemoveAllUserClaims()
    {
        UserClaims.Clear();
    }

    /// <summary>
    /// 删除声明
    /// </summary>
    /// <param name="type"></param>
    public virtual void RemoveClaim(string type)
    {
        UserClaims.RemoveAll(c => c.Type == type);
    }

    /// <summary>
    /// 查找声明
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual ApiResourceClaim? FindClaim(string type)
    {
        return UserClaims.FirstOrDefault(c => c.Type == type);
    }

    /// <summary>
    /// 移除所有凭证
    /// </summary>
    public virtual void RemoveAllSecrets()
    {
        Secrets.Clear();
    }

    /// <summary>
    /// 移除所有范围
    /// </summary>
    public virtual void RemoveAllScopes()
    {
        Scopes.Clear();
    }

    /// <summary>
    /// 移除范围
    /// </summary>
    /// <param name="scope"></param>
    public virtual void RemoveScope(string scope)
    {
        Scopes.RemoveAll(r => r.Scope == scope);
    }

    /// <summary>
    /// 查找范围
    /// </summary>
    /// <param name="scope"></param>
    /// <returns></returns>
    public virtual ApiResourceScope? FindScope(string scope)
    {
        return Scopes.FirstOrDefault(r => r.Scope == scope);
    }

    /// <summary>
    /// 添加属性
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
    /// 移除所有属性
    /// </summary>
    public virtual void RemoveAllProperties()
    {
        Properties.Clear();
    }

    /// <summary>
    /// 移除属性
    /// </summary>
    /// <param name="key"></param>
    public virtual void RemoveProperty(string key)
    {
        Properties.RemoveAll(r => r.Key == key);
    }

    /// <summary>
    /// 查找属性
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual ApiResourceProperty? FindProperty(string key)
    {
        return Properties.FirstOrDefault(r => r.Key == key);
    }
}
