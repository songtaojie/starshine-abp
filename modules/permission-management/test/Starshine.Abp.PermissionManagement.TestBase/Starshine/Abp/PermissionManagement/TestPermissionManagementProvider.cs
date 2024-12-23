using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.PermissionManagement;

public class TestPermissionManagementProvider : PermissionManagementProvider
{
    public override string Name => "Test";

    public TestPermissionManagementProvider(
        IPermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    {

    }
}
