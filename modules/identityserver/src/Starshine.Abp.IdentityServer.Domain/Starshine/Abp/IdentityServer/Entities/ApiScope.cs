using Volo.Abp.Domain.Entities.Auditing;

namespace Starshine.Abp.IdentityServer.Entities;

/// <summary>
/// API 范围
/// </summary>
public class ApiScope : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public virtual bool Enabled { get; set; }

    /// <summary>
    /// 范围名称
    /// </summary>
    public required virtual string Name { get; set; }

    /// <summary>
    /// 范围显示名称
    /// </summary>
    public virtual string? DisplayName { get; set; }

    /// <summary>
    /// 范围描述
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 是否必选
    /// </summary>
    public virtual bool Required { get; set; }

    /// <summary>
    /// 是否强调
    /// </summary>
    public virtual bool Emphasize { get; set; }

    /// <summary>
    /// 是否在发现文档中显示
    /// </summary>
    public virtual bool ShowInDiscoveryDocument { get; set; } = true;

    /// <summary>
    /// 范围声明
    /// </summary>
    public virtual List<ApiScopeClaim> UserClaims { get; protected set; } = [];

    /// <summary>
    /// 范围属性
    /// </summary>
    public virtual List<ApiScopeProperty> Properties { get; protected set; } = [];

    /// <summary>
    /// 构造函数
    /// </summary>
    public ApiScope(Guid id) : base(id)
    {

    }

    /// <summary>
    /// 添加范围声明
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
    /// 移除范围声明
    /// </summary>
    public virtual void RemoveAllUserClaims()
    {
        UserClaims.Clear();
    }

    /// <summary>
    /// 移除范围声明
    /// </summary>
    /// <param name="type"></param>
    public virtual void RemoveClaim(string type)
    {
        UserClaims.RemoveAll(r => r.Type == type);
    }

    /// <summary>
    /// 查找范围声明
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual ApiScopeClaim? FindClaim(string type)
    {
        return UserClaims.FirstOrDefault(r => r.Type == type);
    }

    /// <summary>
    /// 添加范围属性
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
    /// 移除所有范围属性
    /// </summary>
    public virtual void RemoveAllProperties()
    {
        Properties.Clear();
    }

    /// <summary>
    /// 移除范围属性
    /// </summary>
    /// <param name="key"></param>
    public virtual void RemoveProperty(string key)
    {
        Properties.RemoveAll(r => r.Key == key);
    }

    /// <summary>
    /// 查找范围属性
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual ApiScopeProperty? FindProperty(string key)
    {
        return Properties.FirstOrDefault(r => r.Key == key);
    }
}
