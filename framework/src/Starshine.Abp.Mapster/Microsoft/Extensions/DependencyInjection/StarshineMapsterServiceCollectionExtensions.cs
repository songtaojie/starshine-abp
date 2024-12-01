using Microsoft.Extensions.DependencyInjection.Extensions;
using Starshine.Abp.Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StarshineMapsterServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapperObjectMapper(this IServiceCollection services)
        {
            return services.Replace(
                ServiceDescriptor.Transient<IAutoObjectMappingProvider, MapsterAutoObjectMappingProvider>()
            );
        }

        public static IServiceCollection AddAutoMapperObjectMapper<TContext>(this IServiceCollection services)
        {
            return services.Replace(
                ServiceDescriptor.Transient<IAutoObjectMappingProvider<TContext>, MapsterAutoObjectMappingProvider<TContext>>()
            );
        }
    }
}
