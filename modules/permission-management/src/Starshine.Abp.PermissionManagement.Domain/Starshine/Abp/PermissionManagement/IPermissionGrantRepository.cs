using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.PermissionManagement;

public interface IPermissionGrantRepository : IBasicRepository<PermissionGrant, Guid>
{
    Task<PermissionGrant> FindAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<PermissionGrant>> GetListAsync(
        string[] names,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );
}
