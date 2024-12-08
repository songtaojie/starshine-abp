using MapsterMapper;
using Starshine.Abp.Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.ObjectMapping
{
    /// <summary>
    /// MapsterObjectMapper扩展类型
    /// </summary>
    public static class StarshineMapsterObjectMapperExtensions
    {
        /// <summary>
        /// 获取Mapper对象
        /// </summary>
        /// <param name="objectMapper"></param>
        /// <returns></returns>
        public static IMapper GetMapper(this IObjectMapper objectMapper)
        {
            return objectMapper.AutoObjectMappingProvider.GetMapper();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="autoObjectMappingProvider"></param>
        /// <returns></returns>
        /// <exception cref="AbpException"></exception>
        public static IMapper GetMapper(this IAutoObjectMappingProvider autoObjectMappingProvider)
        {
            if (autoObjectMappingProvider is MapsterAutoObjectMappingProvider mapsterAutoObjectMappingProvider)
            {
                return mapsterAutoObjectMappingProvider.MapperAccessor.Mapper;
            }

            throw new AbpException($"Given object is not an instance of {typeof(MapsterAutoObjectMappingProvider).AssemblyQualifiedName}. The type of the given object it {autoObjectMappingProvider.GetType().AssemblyQualifiedName}");
        }
    }
}
