using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.ApiResources;

public class ApiResourceScope : Entity
{
    public virtual Guid ApiResourceId { get; protected set; }

    public virtual string Scope { get; set; } = null!;

    protected ApiResourceScope()
    {

    }

    public virtual bool Equals(Guid apiResourceId, [NotNull] string scope)
    {
        return ApiResourceId == apiResourceId && Scope == scope;
    }

    protected internal ApiResourceScope(
        Guid apiResourceId,
        [NotNull] string scope)
    {
        Check.NotNull(scope, nameof(scope));

        ApiResourceId = apiResourceId;
        Scope = scope;
    }

    public override object[] GetKeys()
    {
        return [ApiResourceId, Scope];
    }
}
