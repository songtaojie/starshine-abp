using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Clients;

public class ClientCorsOrigin : Entity
{
    public virtual Guid ClientId { get; protected set; }

    public virtual string Origin { get; protected set; } = null!;

    protected ClientCorsOrigin()
    {

    }

    public virtual bool Equals(Guid clientId, [NotNull] string uri)
    {
        return ClientId == clientId && Origin == uri;
    }

    protected internal ClientCorsOrigin(Guid clientId, [NotNull] string origin)
    {
        Check.NotNull(origin, nameof(origin));

        ClientId = clientId;
        Origin = origin;
    }

    public override object[] GetKeys()
    {
        return [ClientId, Origin];
    }
}
