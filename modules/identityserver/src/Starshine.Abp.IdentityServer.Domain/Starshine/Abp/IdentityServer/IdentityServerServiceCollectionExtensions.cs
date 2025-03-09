using Starshine.IdentityServer.Services;
using Starshine.IdentityServer.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// 身份服务器服务集合扩展
/// </summary>
public static class IdentityServerServiceCollectionExtensions
{
    /// <summary>
    /// 添加严格重定向Uri验证器
    /// </summary>
    /// <param name="services"></param>
    public static void AddStrictRedirectUriValidator(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<IRedirectUriValidator, StarshineStrictRedirectUriValidator>());
    }

    /// <summary>
    /// 添加客户端配置验证器
    /// </summary>
    /// <param name="services"></param>
    public static void AddClientConfigurationValidator(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<IClientConfigurationValidator, StarshineClientConfigurationValidator>());
    }

    /// <summary>
    /// 添加通配符子域CorsPolicy服务
    /// </summary>
    /// <param name="services"></param>
    public static void AddWildcardSubdomainCorsPolicyService(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Transient<ICorsPolicyService, StarshineWildcardSubdomainCorsPolicyService>());
    }
}
