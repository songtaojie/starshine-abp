using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Starshine.Abp.IdentityServer.Clients;
using Starshine.Abp.IdentityServer.Devices;
using Starshine.Abp.IdentityServer.Grants;

namespace Starshine.Abp.IdentityServer;

public static class IdentityServerBuilderExtensions
{
    public static IIdentityServerBuilder AddAbpStores(this IIdentityServerBuilder builder)
    {
        builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
        builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();

        return builder
            .AddClientStore<ClientStore>()
            .AddResourceStore<ResourceStore>()
            .AddCorsPolicyService<AbpCorsPolicyService>();
    }
}
