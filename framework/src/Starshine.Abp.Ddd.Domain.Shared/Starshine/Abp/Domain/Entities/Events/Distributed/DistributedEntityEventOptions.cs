namespace Starshine.Abp.Domain.Entities.Events.Distributed;
/// <summary>
/// DistributedEntityEventOptions
/// </summary>
public class DistributedEntityEventOptions
{
    /// <summary>
    /// AutoEventSelectors
    /// </summary>
    public IAutoEntityDistributedEventSelectorList AutoEventSelectors { get; }

    /// <summary>
    /// 事件映射
    /// </summary>
    public Dictionary<Type, EtoMappingDictionaryItem> EtoMappings { get; set; }

    /// <summary>
    /// DistributedEntityEventOptions
    /// </summary>
    public DistributedEntityEventOptions()
    {
        AutoEventSelectors = new AutoEntityDistributedEventSelectorList();
        EtoMappings = [];
    }

    /// <summary>
    /// 添加实体ETO映射
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityEto"></typeparam>
    /// <param name="objectMappingContextType"></param>
    public void AddEtoMapping<TEntity, TEntityEto>(Type? objectMappingContextType = null)
    {
        EtoMappings[typeof(TEntity)] = new EtoMappingDictionaryItem(typeof(TEntityEto), objectMappingContextType);
    }
}
