using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// 数据库上下文配置
    /// </summary>
    public interface IStarshineSugarDbContextOptionsBuilder : IAbpCommonDbContextRegistrationOptionsBuilder
    {
    }
}
