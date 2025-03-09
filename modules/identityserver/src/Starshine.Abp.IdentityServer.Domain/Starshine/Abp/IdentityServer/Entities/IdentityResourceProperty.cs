using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;

/// <summary>
/// 身份资源属性
/// </summary>
public class IdentityResourceProperty : Entity
{
    /// <summary>
    /// 身份资源Id
    /// </summary>
    public required virtual Guid IdentityResourceId { get; set; }

    /// <summary>
    /// 身份资源属性键
    /// </summary>
    public required virtual string Key { get; set; }

    /// <summary>
    /// 身份资源属性值
    /// </summary>
    public required virtual string Value { get; set; } 

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal IdentityResourceProperty()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="identityResourceId"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid identityResourceId, string key, string value)
    {
        return IdentityResourceId == identityResourceId && Key == key && Value == value;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [IdentityResourceId, Key];
    }
}
