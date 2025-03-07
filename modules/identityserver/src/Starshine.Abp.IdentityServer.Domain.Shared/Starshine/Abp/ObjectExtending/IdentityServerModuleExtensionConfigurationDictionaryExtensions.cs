using Starshine.Abp.IdentityServer.Consts;
using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Starshine.Abp.ObjectExtending;

/// <summary>
/// 身份服务器模块扩展配置
/// </summary>
public static class IdentityServerModuleExtensionConfigurationDictionaryExtensions
{
    /// <summary>
    /// 配置客户端
    /// </summary>
    /// <param name="modules"></param>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public static ModuleExtensionConfigurationDictionary ConfigureIdentityServer(
        this ModuleExtensionConfigurationDictionary modules,
        Action<IdentityServerModuleExtensionConfiguration> configureAction)
    {
        return modules.ConfigureModule(
            IdentityServerModuleExtensionConsts.ModuleName,
            configureAction
        );
    }
}
