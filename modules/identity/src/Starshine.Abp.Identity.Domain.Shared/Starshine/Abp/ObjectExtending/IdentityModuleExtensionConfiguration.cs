using Volo.Abp.ObjectExtending.Modularity;

namespace Starshine.Abp.ObjectExtending;

/// <summary>
/// 身份模块扩展配置
/// </summary>
public class IdentityModuleExtensionConfiguration : ModuleExtensionConfiguration
{
    /// <summary>
    /// 配置用户
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public IdentityModuleExtensionConfiguration ConfigureUser(Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(IdentityModuleExtensionConsts.EntityNames.User, configureAction);
    }

    /// <summary>
    /// 配置角色
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public IdentityModuleExtensionConfiguration ConfigureRole(Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(IdentityModuleExtensionConsts.EntityNames.Role, configureAction);
    }

    /// <summary>
    /// 配置声明类型
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public IdentityModuleExtensionConfiguration ConfigureClaimType(Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(IdentityModuleExtensionConsts.EntityNames.ClaimType, configureAction);
    }

    /// <summary>
    /// 配置组织单位
    /// </summary>
    /// <param name="configureAction"></param>
    /// <returns></returns>
    public IdentityModuleExtensionConfiguration ConfigureOrganizationUnit(Action<EntityExtensionConfiguration> configureAction)
    {
        return this.ConfigureEntity(IdentityModuleExtensionConsts.EntityNames.OrganizationUnit, configureAction);
    }
}
