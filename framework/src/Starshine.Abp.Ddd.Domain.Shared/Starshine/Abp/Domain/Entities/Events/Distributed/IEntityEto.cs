namespace Starshine.Abp.Domain.Entities.Events.Distributed;

/// <summary>
/// 实体更新事件数据
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntityEto<TKey>
{
    /// <summary>
    /// 此实体的唯一标识符。
    /// </summary>
    TKey Id { get; set; }
}