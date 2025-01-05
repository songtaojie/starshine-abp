using System;
using System.Linq;
using System.Threading.Tasks;
using Starshine.IdentityServer.Configuration;
using Starshine.IdentityServer.Validation;

namespace Starshine.Abp.IdentityServer;

public class StarshineClientConfigurationValidator : DefaultClientConfigurationValidator
{
    public StarshineClientConfigurationValidator(IdentityServerOptions options)
        : base(options)
    {
    }

    protected override Task ValidateAllowedCorsOriginsAsync(ClientConfigurationValidationContext context)
    {
        context.Client.AllowedCorsOrigins = context.Client
            .AllowedCorsOrigins.Select(x => x.Replace("{0}.", string.Empty, StringComparison.OrdinalIgnoreCase))
            .ToHashSet();

        return base.ValidateAllowedCorsOriginsAsync(context);
    }
}
