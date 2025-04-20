using System;
using System.Threading.Tasks;

namespace Starshine.Abp.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext>
    where TDbContext : IEfCoreDbContext
{
    Task<TDbContext> GetDbContextAsync();
}
