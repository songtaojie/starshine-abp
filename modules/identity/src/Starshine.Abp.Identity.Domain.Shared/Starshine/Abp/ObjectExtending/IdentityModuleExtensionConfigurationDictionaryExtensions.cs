using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Starshine.Abp.ObjectExtending;

/// <summary>
/// 身份模块扩展配置字典扩展
/// </summary>
public static class IdentityModuleExtensionConfigurationDictionaryExtensions
{
    /// <summary>
    /// 配置身份
    /// </summary>
    /// <param name="modules"></param>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public static ModuleExtensionConfigurationDictionary ConfigureIdentity(this ModuleExtensionConfigurationDictionary modules,Action<IdentityModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(IdentityModuleExtensionConsts.ModuleName, configureAction);
    }
}
