using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Clients;

public class ClientIdPRestriction : Entity
{
    public virtual Guid ClientId { get; set; }

    public virtual string Provider { get; set; } = null!;

    protected ClientIdPRestriction()
    {

    }

    public virtual bool Equals(Guid clientId, [NotNull] string provider)
    {
        return ClientId == clientId && Provider == provider;
    }

    protected internal ClientIdPRestriction(Guid clientId, [NotNull] string provider)
    {
        Check.NotNull(provider, nameof(provider));

        ClientId = clientId;
        Provider = provider;
    }

    public override object[] GetKeys()
    {
        return [ClientId, Provider];
    }
}
