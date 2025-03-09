using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;

/// <summary>
/// Api 范围属性
/// </summary>
public class ApiScopeProperty : Entity
{
    /// <summary>
    /// Api 范围Id
    /// </summary>
    public virtual Guid ApiScopeId { get; set; }

    /// <summary>
    /// 属性键
    /// </summary>
    public required virtual string Key { get; set; }

    /// <summary>
    /// 属性值
    /// </summary>
    public required virtual string Value { get; set; }

    /// <summary>
    ///   构造函数
    /// </summary>
    protected internal ApiScopeProperty()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="apiScopeId"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid apiScopeId, [NotNull] string key, string value)
    {
        return ApiScopeId == apiScopeId && Key == key && Value == value;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ApiScopeId, Key];
    }
}
