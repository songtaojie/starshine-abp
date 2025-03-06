using Volo.Abp.EventBus.Distributed;

namespace Starshine.Abp.EntityFrameworkCore.DistributedEvents;

public interface IDbContextEventInbox<TDbContext> : IEventInbox
    where TDbContext : IHasEventInbox
{

}
