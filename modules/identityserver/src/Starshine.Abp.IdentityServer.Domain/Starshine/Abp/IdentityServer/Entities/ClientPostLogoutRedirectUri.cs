using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 客户端注销后重定向 Uri
/// </summary>
public class ClientPostLogoutRedirectUri : Entity
{
    /// <summary>
    /// 客户端 Id
    /// </summary>
    public required virtual Guid ClientId { get;  set; }

    /// <summary>
    /// 重定向 Uri
    /// </summary>
    public required virtual string PostLogoutRedirectUri { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ClientPostLogoutRedirectUri()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, string uri)
    {
        return ClientId == clientId && PostLogoutRedirectUri == uri;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ClientId, PostLogoutRedirectUri];
    }
}
