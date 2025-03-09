using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 客户端声明
/// </summary>
public class ClientClaim : Entity
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public required virtual Guid ClientId { get; set; }

    /// <summary>
    /// 客户端声明类型
    /// </summary>
    public required virtual string Type { get; set; }

    /// <summary>
    /// 客户端声明值
    /// </summary>
    public required virtual string Value { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ClientClaim()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, string type, string value)
    {
        return ClientId == clientId && Type == type && Value == value;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ClientId, Type, Value];
    }
}
