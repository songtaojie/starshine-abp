﻿using Starshine.Abp.Application;
using Volo.Abp.Modularity;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// 模块
/// </summary>
[DependsOn(typeof(StarshineTenantManagementDomainModule))]
[DependsOn(typeof(StarshineTenantManagementApplicationContractsModule))]
[DependsOn(typeof(StarshineDddApplicationModule))]
public class StarshineTenantManagementApplicationModule : AbpModule
{
}
