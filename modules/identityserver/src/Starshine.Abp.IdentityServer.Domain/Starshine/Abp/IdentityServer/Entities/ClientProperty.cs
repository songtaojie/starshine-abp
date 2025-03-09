using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 客户端属性
/// </summary>
public class ClientProperty : Entity
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public required virtual Guid ClientId { get; set; }

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
    protected internal ClientProperty()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, string key, string value)
    {
        return ClientId == clientId && Key == key && Value == value;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ClientId, Key];
    }
}
