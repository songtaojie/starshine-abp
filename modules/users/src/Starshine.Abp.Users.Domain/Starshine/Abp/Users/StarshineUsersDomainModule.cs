using Starshine.Abp.Domain;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Users;

/// <summary>
/// 用户领域模块
/// </summary>
[DependsOn(
    typeof(StarshineUsersDomainSharedModule),
    typeof(StarshineUsersAbstractionModule),
    typeof(StarshineDddDomainModule)
    )]
public class StarshineUsersDomainModule : AbpModule
{

}
