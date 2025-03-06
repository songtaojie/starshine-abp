using System;
using System.Collections.Generic;

namespace Starshine.Abp.Domain.Entities.Events.Distributed;

public interface IAutoEntityDistributedEventSelectorList : IList<NamedTypeSelector>
{
}
