using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.ObjectExtending;

/// <summary>
/// 租户管理模块扩展配置字典扩展
/// </summary>
public static class TenantManagementModuleExtensionConfigurationDictionaryExtensions
{
    /// <summary>
    /// 配置租户管理
    /// </summary>
    /// <param name="modules"></param>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public static ModuleExtensionConfigurationDictionary ConfigureTenantManagement(this ModuleExtensionConfigurationDictionary modules,Action<TenantManagementModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            TenantManagementModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}
