using Mapster;
using Volo.Abp.ObjectMapping;

namespace Starshine.Abp.Mapster
{
    /// <summary>
    /// 基于Mapster的自动映射
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class MapsterAutoObjectMappingProvider<TContext> : MapsterAutoObjectMappingProvider, IAutoObjectMappingProvider<TContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapperAccessor"></param>
        public MapsterAutoObjectMappingProvider(IMapperAccessor mapperAccessor) : base(mapperAccessor)
        {
        }
    }
    /// <summary>
    /// 基于Mapster的自动映射
    /// </summary>
    public class MapsterAutoObjectMappingProvider : IAutoObjectMappingProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public IMapperAccessor MapperAccessor { get; }

        /// <summary>
        /// 基于Mapster的自动映射
        /// </summary>
        /// <param name="mapperAccessor"></param>
        public MapsterAutoObjectMappingProvider(IMapperAccessor mapperAccessor)
        {
            MapperAccessor = mapperAccessor;
        }
        /// <summary>
        /// 自动映射
        /// </summary>
        /// <typeparam name="TSource">元数据类型</typeparam>
        /// <typeparam name="TDestination">目标数据类型</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination Map<TSource, TDestination>(object source)
        {
            return MapperAccessor.Mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 自动映射
        /// </summary>
        /// <typeparam name="TSource">元数据类型</typeparam>
        /// <typeparam name="TDestination">目标数据类型</typeparam>
        /// <param name="source">原数据</param>
        /// <param name="destination">目标数据</param>
        /// <returns></returns>
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.Adapt(destination);
        }
    }
}
