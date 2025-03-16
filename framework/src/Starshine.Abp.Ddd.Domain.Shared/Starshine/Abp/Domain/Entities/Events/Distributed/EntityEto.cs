using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace Starshine.Abp.Domain.Entities.Events.Distributed;
/// <summary>
/// EntityEto
/// </summary>
[Serializable]
public class EntityEto : EtoBase
{
    /// <summary>
    /// 实体类型
    /// </summary>
    public required string EntityType { get; set; }

    /// <summary>
    /// 实体主键
    /// </summary>
    public required string KeysAsString { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public EntityEto()
    {

    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="entityType"></param>
    /// <param name="keysAsString"></param>
    public EntityEto(string entityType, string keysAsString)
    {
        EntityType = entityType;
        KeysAsString = keysAsString;
    }
}

/// <summary>
/// EntityEto
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class EntityEto<TKey> : IEntityEto<TKey>
{
    /// <summary>
    /// 实体主键
    /// </summary>
    public required TKey Id { get; set; }
}