using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户抽象模块
/// </summary>
[DependsOn(
    typeof(AbpMultiTenancyModule),
    typeof(AbpEventBusModule)
    )]
public class StarshineUsersAbstractionModule : AbpModule
{

}
