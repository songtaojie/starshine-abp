using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

/// <summary>
/// 租户管理模块扩展配置
/// </summary>
public class TenantManagementModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    /// <summary>
    /// 配置租户
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public TenantManagementModuleExtensionConfiguration ConfigureTenant(Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(TenantManagementModuleExtensionConsts.EntityNames.Tenant,configureAction);
    }
}
