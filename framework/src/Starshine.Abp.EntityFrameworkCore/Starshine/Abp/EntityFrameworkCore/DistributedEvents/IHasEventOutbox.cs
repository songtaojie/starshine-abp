using Microsoft.EntityFrameworkCore;

namespace Starshine.Abp.EntityFrameworkCore.DistributedEvents;

public interface IHasEventOutbox : IEfCoreDbContext
{
    DbSet<OutgoingEventRecord> OutgoingEvents { get; set; }
}
