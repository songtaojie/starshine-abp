using Starshine.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Users.EntityFrameworkCore;

/// <summary>
/// 用户ef模块
/// </summary>
[DependsOn(
    typeof(StarshineUsersDomainModule),
    typeof(StarshineEntityFrameworkCoreModule)
    )]
public class StarshineUsersEntityFrameworkCoreModule : AbpModule
{

}
