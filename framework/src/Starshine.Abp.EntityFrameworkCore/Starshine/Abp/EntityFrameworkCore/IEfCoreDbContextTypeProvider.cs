using System;

namespace Starshine.Abp.EntityFrameworkCore;

public interface IEfCoreDbContextTypeProvider
{
    Type GetDbContextType(Type dbContextType);
}
