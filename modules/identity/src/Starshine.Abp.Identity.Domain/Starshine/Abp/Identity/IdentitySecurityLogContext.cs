using System;
using System.Collections.Generic;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份安全日志上下文
/// </summary>
public class IdentitySecurityLogContext
{
    /// <summary>
    /// 身份
    /// </summary>
    public string? Identity { get; set; }

    /// <summary>
    /// Action
    /// </summary>
    public string? Action { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 客户端id
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, object> ExtraProperties { get; }

    /// <summary>
    /// 
    /// </summary>
    public IdentitySecurityLogContext()
    {
        ExtraProperties = [];
    }

    /// <summary>
    /// 添加额外属性
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual IdentitySecurityLogContext WithProperty(string key, object value)
    {
        ExtraProperties[key] = value;
        return this;
    }

}
