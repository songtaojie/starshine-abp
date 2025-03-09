using System;
using JetBrains.Annotations;

namespace Starshine.Abp.IdentityServer.Entities;

/// <summary>
/// Api 范围声明
/// </summary>
public class ApiScopeClaim : UserClaim
{
    /// <summary>
    /// Api 范围Id
    /// </summary>
    public required Guid ApiScopeId { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ApiScopeClaim()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="apiScopeId"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid apiScopeId, [NotNull] string type)
    {
        return ApiScopeId == apiScopeId && Type == type;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ApiScopeId, Type];
    }
}
