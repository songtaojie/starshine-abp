using Microsoft.Extensions.DependencyInjection;
using Starshine.Abp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;

namespace Starshine.Abp.Mapster
{
    [DependsOn(
    typeof(AbpObjectMappingModule))]
    public class StarshineAbpMapsterModule: StarshineAbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IAutoObjectMappingProvider, MapsterAutoObjectMappingProvider>();
        }
    }
}
