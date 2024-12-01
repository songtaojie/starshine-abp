using Mapster;
using Volo.Abp.ObjectMapping;

namespace Starshine.Abp.Mapster
{

    public class MapsterAutoObjectMappingProvider<TContext> : MapsterAutoObjectMappingProvider, IAutoObjectMappingProvider<TContext>
    {
    }

    public class MapsterAutoObjectMappingProvider : IAutoObjectMappingProvider
    {
        public TDestination Map<TSource, TDestination>(object source)
        {
            return source.Adapt<TDestination>();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.Adapt(destination);
        }
    }
}
