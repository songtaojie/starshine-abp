using Starshine.Abp.IdentityServer.Consts;
using System;
using Volo.Abp.ObjectExtending.Modularity;

namespace Starshine.Abp.ObjectExtending;
/// <summary>
/// 身份服务器模块扩展配置
/// </summary>
public class IdentityServerModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    /// <summary>
    /// 配置客户端
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public IdentityServerModuleExtensionConfiguration ConfigureClient(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            IdentityServerModuleExtensionConsts.EntityNames.Client,
            configureAction
        );
    }

    /// <summary>
    /// 配置Api资源
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public IdentityServerModuleExtensionConfiguration ConfigureApiResource(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            IdentityServerModuleExtensionConsts.EntityNames.ApiResource,
            configureAction
        );
    }

    /// <summary>
    /// 配置Api范围
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public IdentityServerModuleExtensionConfiguration ConfigureApiScope(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            IdentityServerModuleExtensionConsts.EntityNames.ApiScope,
            configureAction
        );
    }

    /// <summary>
    /// 配置身份资源
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public IdentityServerModuleExtensionConfiguration ConfigureIdentityResource(
        Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(
            IdentityServerModuleExtensionConsts.EntityNames.IdentityResource,
            configureAction
        );
    }
}
