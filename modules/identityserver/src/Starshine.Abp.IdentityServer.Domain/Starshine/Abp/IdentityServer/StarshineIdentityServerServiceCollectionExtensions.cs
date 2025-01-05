using Starshine.IdentityServer.Services;
using Starshine.IdentityServer.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Starshine.Abp.IdentityServer;

public static class StarshineIdentityServerServiceCollectionExtensions
{
    public static void AddAbpStrictRedirectUriValidator(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<IRedirectUriValidator, StarshineStrictRedirectUriValidator>());
    }

    public static void AddAbpClientConfigurationValidator(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<IClientConfigurationValidator, StarshineClientConfigurationValidator>());
    }

    public static void AddAbpWildcardSubdomainCorsPolicyService(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<ICorsPolicyService, StarshineWildcardSubdomainCorsPolicyService>());
    }
}
