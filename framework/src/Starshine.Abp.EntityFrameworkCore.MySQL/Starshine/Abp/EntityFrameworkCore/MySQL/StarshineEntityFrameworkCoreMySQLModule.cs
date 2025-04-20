using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace Starshine.Abp.EntityFrameworkCore.MySQL;

[DependsOn(
    typeof(StarshineEntityFrameworkCoreModule)
    )]
public class StarshineEntityFrameworkCoreMySQLModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpSequentialGuidGeneratorOptions>(options =>
        {
            if (options.DefaultSequentialGuidType == null)
            {
                options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
            }
        });
    }
}
