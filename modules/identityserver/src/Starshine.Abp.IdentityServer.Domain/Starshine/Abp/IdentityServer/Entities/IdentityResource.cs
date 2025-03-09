using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 身份资源
/// </summary>
public class IdentityResource : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 身份资源名称
    /// </summary>
    public required virtual string Name { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public virtual string? DisplayName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 是否可用
    /// </summary>
    public virtual bool Enabled { get; set; }

    /// <summary>
    /// 是否必须
    /// </summary>
    public virtual bool Required { get; set; }

    /// <summary>
    /// 是否强调
    /// </summary>
    public virtual bool Emphasize { get; set; }

    /// <summary>
    /// 是否显示在发现文档中
    /// </summary>
    public virtual bool ShowInDiscoveryDocument { get; set; }

    /// <summary>
    /// 身份资源声明
    /// </summary>
    public virtual List<IdentityResourceClaim> UserClaims { get; set; } = [];

    /// <summary>
    /// 资源属性
    /// </summary>
    public virtual List<IdentityResourceProperty> Properties { get; set; } = [];
    /// <summary>
    ///  创建身份资源
    /// </summary>
    protected IdentityResource()
    {

    }

    /// <summary>
    /// 创建身份资源
    /// </summary>
    /// <param name="id"></param>
    public IdentityResource(Guid id) : base(id)
    {
        ShowInDiscoveryDocument = true;
    }

    /// <summary>
    /// 添加身份资源声明
    /// </summary>
    /// <param name="type"></param>
    public virtual void AddUserClaim([NotNull] string type)
    {
        UserClaims.Add(new IdentityResourceClaim
        {
            IdentityResourceId = Id,
            Type = type
        });
    }

    /// <summary>
    /// 移除所有身份资源声明
    /// </summary>
    public virtual void RemoveAllUserClaims()
    {
        UserClaims.Clear();
    }

    /// <summary>
    /// 移除身份资源声明
    /// </summary>
    /// <param name="type"></param>
    public virtual void RemoveUserClaim(string type)
    {
        UserClaims.RemoveAll(c => c.Type == type);
    }

    /// <summary>
    /// 查找身份资源声明
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual IdentityResourceClaim? FindUserClaim(string type)
    {
        return UserClaims.FirstOrDefault(c => c.Type == type);
    }

    /// <summary>
    /// 添加资源属性
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public virtual void AddProperty(string key, string value)
    {
        var property = FindProperty(key);
        if (property == null)
        {
            Properties.Add(new IdentityResourceProperty
            {
                IdentityResourceId = Id,
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
    /// 移除所有资源属性
    /// </summary>
    public virtual void RemoveAllProperties()
    {
        Properties.Clear();
    }

    /// <summary>
    /// 移除资源属性
    /// </summary>
    /// <param name="key"></param>
    public virtual void RemoveProperty(string key)
    {
        Properties.RemoveAll(r => r.Key == key);
    }

    /// <summary>
    /// 查找资源属性
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual IdentityResourceProperty? FindProperty(string key)
    {
        return Properties.FirstOrDefault(r => r.Key == key);
    }
}
