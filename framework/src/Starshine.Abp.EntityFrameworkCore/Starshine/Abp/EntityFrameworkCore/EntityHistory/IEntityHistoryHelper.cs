using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp.Auditing;
using Starshine.Abp.EntityFrameworkCore.ChangeTrackers;

namespace Starshine.Abp.EntityFrameworkCore.EntityHistory;

public interface IEntityHistoryHelper
{
    void InitializeNavigationHelper(AbpEfCoreNavigationHelper abpEfCoreNavigationHelper);

    List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries);

    void UpdateChangeList(List<EntityChangeInfo> entityChanges);
}
