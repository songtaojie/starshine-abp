﻿using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Clients;

public class ClientGrantType : Entity
{
    public virtual Guid ClientId { get; protected set; }

    public virtual string GrantType { get; protected set; } = null!;

    protected ClientGrantType()
    {

    }

    public virtual bool Equals(Guid clientId, [NotNull] string grantType)
    {
        return ClientId == clientId && GrantType == grantType;
    }

    protected internal ClientGrantType(Guid clientId, [NotNull] string grantType)
    {
        Check.NotNull(grantType, nameof(grantType));

        ClientId = clientId;
        GrantType = grantType;
    }

    public override object[] GetKeys()
    {
        return [ClientId, GrantType];
    }
}
