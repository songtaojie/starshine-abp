using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;

/// <summary>
///Api 资源范围
/// </summary>
public class ApiResourceScope : Entity
{
    /// <summary>
    /// Api 资源Id
    /// </summary>
    public required virtual Guid ApiResourceId { get; set; }

    /// <summary>
    /// 范围
    /// </summary>
    public required virtual string Scope { get; set; } 

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ApiResourceScope()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="apiResourceId"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid apiResourceId, string scope)
    {
        return ApiResourceId == apiResourceId && Scope == scope;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ApiResourceId, Scope];
    }
}
