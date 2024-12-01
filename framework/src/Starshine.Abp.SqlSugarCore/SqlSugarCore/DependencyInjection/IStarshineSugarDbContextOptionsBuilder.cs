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
    public interface IStarshineSugarDbContextOptionsBuilder : IAbpCommonDbContextRegistrationOptionsBuilder
    {
    }
}
