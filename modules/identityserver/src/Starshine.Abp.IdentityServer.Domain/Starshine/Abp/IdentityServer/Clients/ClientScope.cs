﻿using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Clients;

public class ClientScope : Entity
{
    public virtual Guid ClientId { get; protected set; }

    public virtual string Scope { get; protected set; } = null!;

    protected ClientScope()
    {

    }

    public virtual bool Equals(Guid clientId, [NotNull] string scope)
    {
        return ClientId == clientId && Scope == scope;
    }

    protected internal ClientScope(Guid clientId, string scope)
    {
        ClientId = clientId;
        Scope = scope;
    }

    public override object[] GetKeys()
    {
        return [ClientId, Scope];
    }
}
