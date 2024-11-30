using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.SqlSugarCore.Repositories
{
    public interface ISqlSugarRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {

    }
}
