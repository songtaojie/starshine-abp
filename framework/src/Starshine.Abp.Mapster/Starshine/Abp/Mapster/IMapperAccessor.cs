using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.Mapster
{
    /// <summary>
    /// IMapper访问接口
    /// </summary>
    public interface IMapperAccessor
    {
        /// <summary>
        /// IMapper对象
        /// </summary>
        IMapper Mapper { get; }
    }
}
