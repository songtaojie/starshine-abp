using System;
using Starshine.IdentityServer;
using JetBrains.Annotations;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 客户端凭证
/// </summary>
public class ClientSecret : Secret
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public required virtual Guid ClientId { get; set; }

    /// <summary>
    /// 客户端凭证
    /// </summary>
    protected internal ClientSecret()
    {
        Type = IdentityServerConstants.SecretTypes.SharedSecret;
    }

    /// <summary>
    /// 客户端凭证
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, [NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
    {
        return ClientId == clientId && Value == value && Type == type;
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
