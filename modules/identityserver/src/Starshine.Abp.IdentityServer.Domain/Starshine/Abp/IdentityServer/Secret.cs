using System;
using Starshine.IdentityServer;
using Starshine.IdentityServer.Models;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp;

namespace Starshine.Abp.IdentityServer;

public abstract class Secret : Entity
{
    public virtual string Type { get; protected set; } = null!;

    public virtual string Value { get; set; } = null!;

    public virtual string? Description { get; set; }

    public virtual DateTimeOffset? Expiration { get; set; }

    protected Secret()
    {

    }

    protected Secret(
        [NotNull] string value,
        DateTimeOffset? expiration = null,
        string type = IdentityServerConstants.SecretTypes.SharedSecret,
        string? description = null)
    {
        Check.NotNull(value, nameof(value));

        Value = value;
        Expiration = expiration;
        Type = type;
        Description = description;
    }
}
