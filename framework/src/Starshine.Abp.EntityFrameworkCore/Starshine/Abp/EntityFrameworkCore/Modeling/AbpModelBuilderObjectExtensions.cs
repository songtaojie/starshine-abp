using Microsoft.EntityFrameworkCore;
using Starshine.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending;

namespace Starshine.Abp.EntityFrameworkCore.Modeling;

public static class AbpModelBuilderObjectExtensions
{
    public static void TryConfigureObjectExtensions<TDbContext>(this ModelBuilder modelBuilder)
        where TDbContext : DbContext
    {
        ObjectExtensionManager.Instance.ConfigureEfCoreDbContext<TDbContext>(modelBuilder);
    }
}
