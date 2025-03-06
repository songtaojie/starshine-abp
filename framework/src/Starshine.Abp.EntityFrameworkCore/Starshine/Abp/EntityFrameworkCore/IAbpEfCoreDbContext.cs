namespace Starshine.Abp.EntityFrameworkCore;

public interface IAbpEfCoreDbContext : IEfCoreDbContext
{
    void Initialize(AbpEfCoreDbContextInitializationContext initializationContext);
}
