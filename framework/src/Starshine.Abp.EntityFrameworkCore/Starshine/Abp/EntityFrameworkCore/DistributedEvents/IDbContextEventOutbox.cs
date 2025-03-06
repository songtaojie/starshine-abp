using Volo.Abp.EventBus.Distributed;

namespace Starshine.Abp.EntityFrameworkCore.DistributedEvents;

public interface IDbContextEventOutbox<TDbContext> : IEventOutbox
    where TDbContext : IHasEventOutbox
{

}
