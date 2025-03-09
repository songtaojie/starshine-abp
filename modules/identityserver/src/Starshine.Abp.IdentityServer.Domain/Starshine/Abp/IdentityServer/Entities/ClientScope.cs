using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 客户端作用域
/// </summary>
public class ClientScope : Entity
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public required virtual Guid ClientId { get; set; }

    /// <summary>
    /// 作用域
    /// </summary>
    public required virtual string Scope { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ClientScope()
    {

    }

    /// <summary>
    /// 比较
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, [NotNull] string scope)
    {
        return ClientId == clientId && Scope == scope;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ClientId, Scope];
    }
}
