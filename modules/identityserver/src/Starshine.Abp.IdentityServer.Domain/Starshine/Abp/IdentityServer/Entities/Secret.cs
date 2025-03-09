using System;
using Starshine.IdentityServer;
using Starshine.IdentityServer.Models;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 凭证
/// </summary>
public abstract class Secret : Entity
{
    /// <summary>
    /// 凭证类型
    /// </summary>
    public virtual string Type { get; set; } = IdentityServerConstants.SecretTypes.SharedSecret;

    /// <summary>
    /// 凭证值
    /// </summary>
    public required virtual string Value { get; set; }

    /// <summary>
    /// 凭证描述
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 凭证过期时间
    /// </summary>
    public virtual DateTimeOffset? Expiration { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected Secret()
    {

    }
}
