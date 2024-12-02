using Mapster;
using Volo.Abp.ObjectMapping;

namespace Starshine.Abp.Mapster
{

    public class MapsterAutoObjectMappingProvider<TContext> : MapsterAutoObjectMappingProvider, IAutoObjectMappingProvider<TContext>
    {
        public MapsterAutoObjectMappingProvider(IMapperAccessor mapperAccessor) : base(mapperAccessor)
        {
        }
    }

    public class MapsterAutoObjectMappingProvider : IAutoObjectMappingProvider
    {

        public IMapperAccessor MapperAccessor { get; }

        public MapsterAutoObjectMappingProvider(IMapperAccessor mapperAccessor)
        {
            MapperAccessor = mapperAccessor;
        }
        public TDestination Map<TSource, TDestination>(object source)
        {
            return MapperAccessor.Mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.Adapt(destination);
        }
    }
}
