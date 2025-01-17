﻿using System;
using Starshine.IdentityServer;
using JetBrains.Annotations;

namespace Starshine.Abp.IdentityServer.ApiResources;

public class ApiResourceSecret : Secret
{
    public virtual Guid ApiResourceId { get; protected set; }

    protected ApiResourceSecret()
    {

    }

    public virtual bool Equals(Guid apiResourceId, [NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
    {
        return ApiResourceId == apiResourceId && Value == value && Type == type;
    }

    protected internal ApiResourceSecret(
        Guid apiResourceId,
        [NotNull] string value,
        DateTimeOffset? expiration = null,
        string type = IdentityServerConstants.SecretTypes.SharedSecret,
        string? description = null
        ) : base(
            value,
            expiration,
            type,
            description)
    {
        ApiResourceId = apiResourceId;
    }

    public override object[] GetKeys()
    {
        return new object[] { ApiResourceId, Type, Value };
    }
}
