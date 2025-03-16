using Starshine.Abp.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.AspNetCore.Serilog;

[DependsOn(
    typeof(AbpMultiTenancyModule),
    typeof(StarshineAspNetCoreModule)
)]
public class StarshineAspNetCoreSerilogModule : AbpModule
{
}
