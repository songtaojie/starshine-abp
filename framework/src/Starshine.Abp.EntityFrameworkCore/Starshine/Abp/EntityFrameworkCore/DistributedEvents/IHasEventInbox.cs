using Microsoft.EntityFrameworkCore;

namespace Starshine.Abp.EntityFrameworkCore.DistributedEvents;

public interface IHasEventInbox : IEfCoreDbContext
{
    DbSet<IncomingEventRecord> IncomingEvents { get; set; }
}
