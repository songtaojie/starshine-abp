using System.Collections.ObjectModel;
using Volo.Abp.Uow;

namespace Starshine.Abp.Domain.Entities;

/// <summary>
/// 基本聚合根实现。
/// </summary>
[Serializable]
public abstract class BasicAggregateRoot : Entity,IAggregateRoot, IDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    /// <summary>
    /// 获取本地事件。
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents;
    }

    /// <summary>
    /// 获取分布式事件。
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents;
    }

    /// <summary>
    /// 清除本地事件。
    /// </summary>
    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    /// <summary>
    /// 清除分布式事件。
    /// </summary>
    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }

    /// <summary>
    /// 添加本地事件。
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    /// <summary>
    /// 添加分布式事件。
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}

/// <summary>
/// 基本聚合根实现。
/// </summary>  
/// <typeparam name="TKey">主键</typeparam>
[Serializable]
public abstract class BasicAggregateRoot<TKey> : Entity<TKey>,
    IAggregateRoot<TKey>,
    IDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    /// <summary>
    /// 构造函数。
    /// </summary>
    protected BasicAggregateRoot()
    {

    }
    /// <summary>
    /// 构造函数。
    /// </summary>
    protected BasicAggregateRoot(TKey id)
        : base(id)
    {

    }

    /// <summary>
    /// 获取本地事件。
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents;
    }

    /// <summary>
    /// 获取分布式事件。
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents;
    }

    /// <summary>
    /// 清除本地事件。
    /// </summary>
    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    /// <summary>
    /// 清除分布式事件。
    /// </summary>
    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }

    /// <summary>
    ///  添加本地事件。
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    /// <summary>
    /// 添加分布式事件。
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}
