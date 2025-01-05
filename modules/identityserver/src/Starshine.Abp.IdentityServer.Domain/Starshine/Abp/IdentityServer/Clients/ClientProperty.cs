using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Clients;

public class ClientProperty : Entity
{
    public virtual Guid ClientId { get; set; }

    public virtual string Key { get; set; } = null!;

    public virtual string Value { get; set; } = null!;

    protected ClientProperty()
    {

    }

    public virtual bool Equals(Guid clientId, [NotNull] string key, [NotNull] string value)
    {
        return ClientId == clientId && Key == key && Value == value;
    }

    protected internal ClientProperty(Guid clientId, [NotNull] string key, [NotNull] string value)
    {
        Check.NotNull(key, nameof(key));

        ClientId = clientId;
        Key = key;
        Value = value;
    }

    public override object[] GetKeys()
    {
        return [ClientId, Key];
    }
}
