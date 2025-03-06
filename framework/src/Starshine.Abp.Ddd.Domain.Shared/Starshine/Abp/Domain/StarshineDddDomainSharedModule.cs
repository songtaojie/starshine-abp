using Volo.Abp.EventBus.Abstractions;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Domain;

/// <summary>
/// Starshine Ddd 领域共享模块
/// </summary>
[DependsOn(
    typeof(AbpMultiTenancyAbstractionsModule),
    typeof(AbpEventBusAbstractionsModule)
)]
public class StarshineDddDomainSharedModule : AbpModule
{
}
