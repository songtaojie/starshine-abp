using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Users.EntityFrameworkCore;

/// <summary>
/// 用户ef模块
/// </summary>
[DependsOn(
    typeof(StarshineUsersDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class StarshineUsersEntityFrameworkCoreModule : AbpModule
{

}
