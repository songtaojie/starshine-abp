using System.Collections.Generic;

namespace Starshine.Abp.Domain.Entities;
/// <summary>
///域的事件
/// </summary>
public interface IDomainEvents
{
    /// <summary>
    /// 获取本地事件
    /// </summary>
    /// <returns></returns>
    IEnumerable<DomainEventRecord> GetLocalEvents();

    /// <summary>
    /// 获取分布式事件
    /// </summary>
    /// <returns></returns>
    IEnumerable<DomainEventRecord> GetDistributedEvents();

    /// <summary>
    /// 清除本地事件
    /// </summary>
    void ClearLocalEvents();

    /// <summary>
    /// 清除分布式事件
    /// </summary>
    void ClearDistributedEvents();
}
