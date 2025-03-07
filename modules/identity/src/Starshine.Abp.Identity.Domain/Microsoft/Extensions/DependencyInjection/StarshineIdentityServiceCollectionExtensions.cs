using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Starshine.Abp.Identity;
using Starshine.Abp.Identity.Managers;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 扩展方法
/// </summary>
public static class StarshineIdentityServiceCollectionExtensions
{
    /// <summary>
    /// 添加身份服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IdentityBuilder AddStarshineIdentity(this IServiceCollection services)
    {
        return services.AddStarshineIdentity(setupAction: options => { });
    }
    /// <summary>
    /// 添加身份服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="setupAction"></param>
    /// <returns></returns>
    public static IdentityBuilder AddStarshineIdentity(this IServiceCollection services, Action<IdentityOptions> setupAction)
    {
        //AbpRoleManager
        services.TryAddScoped<IdentityRoleManager>();
        services.TryAddScoped(typeof(RoleManager<IdentityRole>), provider => provider.GetRequiredService(typeof(IdentityRoleManager)));

        //AbpUserManager
        services.TryAddScoped<IdentityUserManager>();
        services.TryAddScoped(typeof(UserManager<IdentityUser>), provider => provider.GetRequiredService(typeof(IdentityUserManager)));

        //AbpUserStore
        services.TryAddScoped<IdentityUserStore>();
        services.TryAddScoped(typeof(IUserStore<IdentityUser>), provider => provider.GetRequiredService(typeof(IdentityUserStore)));

        //AbpRoleStore
        services.TryAddScoped<IdentityRoleStore>();
        services.TryAddScoped(typeof(IRoleStore<IdentityRole>), provider => provider.GetRequiredService(typeof(IdentityRoleStore)));

        return services
            .AddIdentityCore<IdentityUser>(setupAction)
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<StarshineUserClaimsPrincipalFactory>();
    }
}
