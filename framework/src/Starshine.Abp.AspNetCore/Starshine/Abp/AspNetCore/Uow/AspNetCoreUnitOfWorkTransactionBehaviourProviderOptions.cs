using System.Collections.Generic;

namespace Starshine.Abp.AspNetCore.Uow;

public class AspNetCoreUnitOfWorkTransactionBehaviourProviderOptions
{
    public List<string> NonTransactionalUrls { get; }

    public AspNetCoreUnitOfWorkTransactionBehaviourProviderOptions()
    {
        NonTransactionalUrls = new List<string>
            {
                "/connect/"
            };
    }
}
