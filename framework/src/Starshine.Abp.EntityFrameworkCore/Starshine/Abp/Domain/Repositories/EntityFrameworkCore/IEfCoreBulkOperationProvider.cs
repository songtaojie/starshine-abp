using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Starshine.Abp.EntityFrameworkCore;
using Starshine.Abp.Domain.Entities;

namespace Starshine.Abp.Domain.Repositories.EntityFrameworkCore;

public interface IEfCoreBulkOperationProvider
{
    Task InsertManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity;


    Task UpdateManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity;


    Task DeleteManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken
    )
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity;
}
