using System;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Grants;

public class PersistedGrant : AggregateRoot<Guid>
{
    public virtual string Key { get; set; } = null!;    

    public virtual string Type { get; set; } = null!;

    public virtual string? SubjectId { get; set; }

    public virtual string? SessionId { get; set; }

    public virtual string? ClientId { get; set; }

    public virtual string? Description { get; set; }

    public virtual DateTimeOffset CreationTime { get; set; }

    public virtual DateTimeOffset? Expiration { get; set; }

    public virtual DateTimeOffset? ConsumedTime { get; set; }

    public virtual string? Data { get; set; }

    protected PersistedGrant()
    {
    }

    public PersistedGrant(Guid id)
        : base(id)
    {
    }
}
