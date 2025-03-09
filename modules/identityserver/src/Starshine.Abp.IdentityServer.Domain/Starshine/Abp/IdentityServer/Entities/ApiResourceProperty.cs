using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// API资源属性
/// </summary>
public class ApiResourceProperty : Entity
{
    /// <summary>
    /// API资源Id
    /// </summary>
    public required virtual Guid ApiResourceId { get; set; }

    /// <summary>
    /// 属性键
    /// </summary>
    public required virtual string Key { get; set; }

    /// <summary>
    /// 属性值
    /// </summary>
    public required virtual string Value { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ApiResourceProperty()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="aiResourceId"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid aiResourceId, [NotNull] string key, string value)
    {
        return ApiResourceId == aiResourceId && Key == key && Value == value;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ApiResourceId, Key];
    }
}
