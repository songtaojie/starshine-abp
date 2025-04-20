using Starshine.Abp.Domain.Repositories;
using Starshine.Abp.Domain.Repositories.EntityFrameworkCore;

namespace Starshine.Abp.EntityFrameworkCore.DependencyInjection;

public class EfCoreRepositoryRegistrar : RepositoryRegistrarBase<AbpDbContextRegistrationOptions>
{
    public EfCoreRepositoryRegistrar(AbpDbContextRegistrationOptions options)
        : base(options)
    {

    }

    protected override IEnumerable<Type> GetEntityTypes(Type dbContextType)
    {
        return DbContextHelper.GetEntityTypes(dbContextType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType)
    {
        return typeof(EfCoreRepository<,>).MakeGenericType(dbContextType, entityType);
    }

    protected override Type GetRepositoryType(Type dbContextType, Type entityType, Type primaryKeyType)
    {
        return typeof(EfCoreRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);
    }
}
