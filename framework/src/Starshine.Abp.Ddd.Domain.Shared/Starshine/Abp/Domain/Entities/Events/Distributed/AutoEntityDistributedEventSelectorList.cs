using System.Collections.Generic;
using Volo.Abp;

namespace Starshine.Abp.Domain;
/// <summary>
/// 自动实体分布式事件选择器列表
/// </summary>
public class AutoEntityDistributedEventSelectorList : List<NamedTypeSelector>, IAutoEntityDistributedEventSelectorList
{

}
