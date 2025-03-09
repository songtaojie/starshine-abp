using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;

/// <summary>
/// 客户端重定向地址
/// </summary>
public class ClientRedirectUri : Entity
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public required virtual Guid ClientId { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    public required virtual string RedirectUri { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ClientRedirectUri()
    {

    }

    /// <summary>
    /// 判断重定向地址是否相等
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, string uri)
    {
        return ClientId == clientId && RedirectUri == uri;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ClientId, RedirectUri];
    }
}
