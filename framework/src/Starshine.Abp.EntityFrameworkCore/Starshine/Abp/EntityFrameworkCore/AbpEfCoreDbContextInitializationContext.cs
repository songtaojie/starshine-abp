using Volo.Abp.Uow;

namespace Starshine.Abp.EntityFrameworkCore;

public class AbpEfCoreDbContextInitializationContext
{
    public IUnitOfWork UnitOfWork { get; }

    public AbpEfCoreDbContextInitializationContext(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}
