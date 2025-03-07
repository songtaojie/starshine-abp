using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;

public class ApiResourceProperty : Entity
{
    public virtual Guid ApiResourceId { get; protected set; }

    public virtual string Key { get; set; } = null!;

    public virtual string Value { get; set; } = null!;

    protected ApiResourceProperty()
    {

    }

    public virtual bool Equals(Guid aiResourceId, [NotNull] string key, string value)
    {
        return ApiResourceId == aiResourceId && Key == key && Value == value;
    }

    protected internal ApiResourceProperty(Guid aiResourceId, [NotNull] string key, [NotNull] string value)
    {
        Check.NotNull(key, nameof(key));

        ApiResourceId = aiResourceId;
        Key = key;
        Value = value;
    }

    public override object[] GetKeys()
    {
        return [ApiResourceId, Key];
    }
}
