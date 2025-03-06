using System;
using System.Threading.Tasks;

namespace Starshine.Abp.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext>
    where TDbContext : IEfCoreDbContext
{
    [Obsolete("Use GetDbContextAsync method.")]
    TDbContext GetDbContext();

    Task<TDbContext> GetDbContextAsync();
}
