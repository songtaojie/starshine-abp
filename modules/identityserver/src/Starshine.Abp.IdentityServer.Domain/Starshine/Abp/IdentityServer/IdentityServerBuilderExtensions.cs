using Starshine.IdentityServer.Stores;
using Microsoft.Extensions.DependencyInjection;
using Starshine.Abp.IdentityServer.Stores;

namespace Starshine.Abp.IdentityServer;

/// <summary>
/// IdentityServerBuilder扩展
/// </summary>
public static class IdentityServerBuilderExtensions
{
    /// <summary>
    /// 添加存储
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IIdentityServerBuilder AddStores(this IIdentityServerBuilder builder)
    {
        builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
        builder.Services.AddTransient<IDeviceFlowStore, DeviceFlowStore>();

        return builder
            .AddClientStore<ClientStore>()
            .AddResourceStore<ResourceStore>()
            .AddCorsPolicyService<StarshineCorsPolicyService>();
    }
}
