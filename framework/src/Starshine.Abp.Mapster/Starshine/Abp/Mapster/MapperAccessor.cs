using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.Mapster
{
    /// <summary>
    /// MapperAccessor实现
    /// </summary>
    internal class MapperAccessor : IMapperAccessor
    {
        public IMapper Mapper { get; set; } = default!;
    }
}
