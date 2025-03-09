using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 客户端 ID 限制
/// </summary>
public class ClientIdPRestriction : Entity
{
    /// <summary>
    /// 客户端id
    /// </summary>
    public required virtual Guid ClientId { get; set; }

    /// <summary>
    /// 身份提供商
    /// </summary>
    public required virtual string Provider { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ClientIdPRestriction()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, string provider)
    {
        return ClientId == clientId && Provider == provider;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ClientId, Provider];
    }
}
