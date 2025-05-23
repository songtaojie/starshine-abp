﻿using Starshine.Abp.Domain;
using Starshine.Abp.Domain.Entities.Events.Distributed;
using Starshine.Abp.TenantManagement.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Starshine.Abp.TenantManagement;

/// <summary>
/// 租户管理域模块
/// </summary>
[DependsOn(typeof(AbpMultiTenancyModule))]
[DependsOn(typeof(StarshineTenantManagementDomainSharedModule))]
[DependsOn(typeof(StarshineDddDomainModule))]
public class StarshineTenantManagementDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<Tenant, TenantEto>();
        });
    }

    /// <summary>
    /// 配置服务后
    /// </summary>
    /// <param name="context"></param>
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                TenantManagementModuleExtensionConsts.ModuleName,
                TenantManagementModuleExtensionConsts.EntityNames.Tenant,
                typeof(Tenant)
            );
        });
    }
}
