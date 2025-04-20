using Volo.Abp.Modularity;

namespace Starshine.Abp.EntityFrameworkCore.Sqlite;

[DependsOn(
    typeof(StarshineEntityFrameworkCoreModule)
)]
public class StarshineEntityFrameworkCoreSqliteModule : AbpModule
{

}
