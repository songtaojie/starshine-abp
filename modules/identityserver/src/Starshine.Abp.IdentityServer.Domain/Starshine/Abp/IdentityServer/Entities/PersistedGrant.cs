using System;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 持久化授予
/// </summary>
public class PersistedGrant : AggregateRoot<Guid>
{
    /// <summary>
    /// 键
    /// </summary>
    public required virtual string Key { get; set; }

    /// <summary>
    /// 类型  
    /// </summary>
    public required virtual string Type { get; set; }

    /// <summary>
    /// 主体
    /// </summary>
    public virtual string? SubjectId { get; set; }

    /// <summary>
    /// 会话
    /// </summary>
    public virtual string? SessionId { get; set; }

    /// <summary>
    /// 客户端
    /// </summary>
    public required virtual string ClientId { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public virtual DateTimeOffset CreationTime { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public virtual DateTimeOffset? Expiration { get; set; }

    /// <summary>
    /// 消耗时间
    /// </summary>
    public virtual DateTimeOffset? ConsumedTime { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public virtual string? Data { get; set; }

    /// <summary>
    /// 默认构造函数
    /// </summary>
    protected internal PersistedGrant()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="id"></param>
    public PersistedGrant(Guid id)
        : base(id)
    {
    }
}
