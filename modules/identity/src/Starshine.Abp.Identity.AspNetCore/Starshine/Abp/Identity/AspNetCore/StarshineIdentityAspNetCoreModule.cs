using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Identity.AspNetCore;

/// <summary>
/// 
/// </summary>
[DependsOn(
    typeof(StarshineIdentityDomainModule)
    )]
public class StarshineIdentityAspNetCoreModule : AbpModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IdentityBuilder>(builder =>
        {
            builder
                .AddDefaultTokenProviders()
                .AddTokenProvider<LinkUserTokenProvider>(LinkUserTokenProviderConsts.LinkUserTokenProviderName)
                .AddSignInManager<StarshineSignInManager>()
                .AddUserValidator<StarshineIdentityUserValidator>();
        });
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //提取扩展方法，如 IdentityBuilder.AddStarshineSecurityStampValidator()
        context.Services.AddScoped<StarshineSecurityStampValidator>();
        context.Services.AddScoped(typeof(SecurityStampValidator<IdentityUser>), provider => provider.GetRequiredService(typeof(StarshineSecurityStampValidator)));
        context.Services.AddScoped(typeof(ISecurityStampValidator), provider => provider.GetRequiredService(typeof(StarshineSecurityStampValidator)));

        var options = context.Services.ExecutePreConfiguredActions(new StarshineIdentityAspNetCoreOptions());

        if (options.ConfigureAuthentication)
        {
            context.Services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddOptions<SecurityStampValidatorOptions>()
            .Configure<IServiceProvider>((securityStampValidatorOptions, serviceProvider) =>
            {
                var abpRefreshingPrincipalOptions = serviceProvider.GetRequiredService<IOptions<StarshineRefreshingPrincipalOptions>>().Value;
                securityStampValidatorOptions.UpdatePrincipal(abpRefreshingPrincipalOptions);
            });
    }
}
