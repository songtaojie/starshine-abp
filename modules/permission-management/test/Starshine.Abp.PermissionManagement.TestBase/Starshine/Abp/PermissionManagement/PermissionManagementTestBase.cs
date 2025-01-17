﻿using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Starshine.Abp.PermissionManagement;

public abstract class PermissionManagementTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
