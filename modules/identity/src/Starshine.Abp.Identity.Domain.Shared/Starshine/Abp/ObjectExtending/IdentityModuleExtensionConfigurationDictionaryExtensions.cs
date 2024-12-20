using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Starshine.Abp.ObjectExtending;

public static class IdentityModuleExtensionConfigurationDictionaryExtensions
{
    public static ModuleExtensionConfigurationDictionary ConfigureIdentity(
        this ModuleExtensionConfigurationDictionary modules,
        Action<IdentityModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            IdentityModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}
