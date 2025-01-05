using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Clients;

public class ClientClaim : Entity
{
    public virtual Guid ClientId { get; set; }

    public virtual string Type { get; set; } = null!;

    public virtual string Value { get; set; } = null!;

    protected ClientClaim()
    {

    }

    protected internal ClientClaim(Guid clientId, [NotNull] string type, string value)
    {
        Check.NotNull(type, nameof(type));

        ClientId = clientId;
        Type = type;
        Value = value;
    }

    public virtual bool Equals(Guid clientId, string type, string value)
    {
        return ClientId == clientId && Type == type && Value == value;
    }

    public override object[] GetKeys()
    {
        return [ClientId, Type, Value];
    }
}
