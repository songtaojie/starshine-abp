using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户领域模块
/// </summary>
[DependsOn(
    typeof(StarshineUsersDomainSharedModule),
    typeof(StarshineUsersAbstractionModule),
    typeof(AbpDddDomainModule)
    )]
public class StarshineUsersDomainModule : AbpModule
{

}
