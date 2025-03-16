using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.AspNetCore.MultiTenancy;

public static class TenantResolveContextExtensions
{
    public static AspNetCoreMultiTenancyOptions GetAbpAspNetCoreMultiTenancyOptions(this ITenantResolveContext context)
    {
        return context.ServiceProvider.GetRequiredService<IOptions<AspNetCoreMultiTenancyOptions>>().Value;
    }
}
