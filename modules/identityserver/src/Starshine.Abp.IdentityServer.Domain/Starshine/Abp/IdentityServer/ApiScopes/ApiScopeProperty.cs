using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.ApiScopes;

public class ApiScopeProperty : Entity
{
    public virtual Guid ApiScopeId { get; set; }

    public virtual string Key { get; set; } = null!;

    public virtual string Value { get; set; } = null!;

    protected ApiScopeProperty()
    {

    }

    public virtual bool Equals(Guid apiScopeId, [NotNull] string key, string value)
    {
        return ApiScopeId == apiScopeId && Key == key && Value == value;
    }

    protected internal ApiScopeProperty(Guid apiScopeId, [NotNull] string key, [NotNull] string value)
    {
        Check.NotNull(key, nameof(key));

        ApiScopeId = apiScopeId;
        Key = key;
        Value = value;
    }

    public override object[] GetKeys()
    {
        return [ApiScopeId, Key];
    }
}
