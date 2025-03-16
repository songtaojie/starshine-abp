using Starshine.Abp.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.AspNetCore.MultiTenancy;

[DependsOn(
    typeof(AbpMultiTenancyModule),
    typeof(StarshineAspNetCoreModule)
    )]
public class StarshineAspNetCoreMultiTenancyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpTenantResolveOptions>(options =>
        {
            options.TenantResolvers.Add(new QueryStringTenantResolveContributor());
            options.TenantResolvers.Add(new RouteTenantResolveContributor());
            options.TenantResolvers.Add(new HeaderTenantResolveContributor());
            options.TenantResolvers.Add(new CookieTenantResolveContributor());
        });
    }
}
