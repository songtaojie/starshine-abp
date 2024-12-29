namespace Starshine.Abp.ObjectExtending;

/// <summary>
/// 身份模块扩展常量
/// </summary>
public static class IdentityModuleExtensionConsts
{
    /// <summary>
    /// 模块名称
    /// </summary>
    public const string ModuleName = "Identity";

    /// <summary>
    /// 实体名称
    /// </summary>
    public static class EntityNames
    {
        /// <summary>
        /// 用户
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// 角色
        /// </summary>
        public const string Role = "Role";

        /// <summary>
        /// 声明类型
        /// </summary>
        public const string ClaimType = "ClaimType";

        /// <summary>
        /// 组织单位
        /// </summary>
        public const string OrganizationUnit = "OrganizationUnit";
    }

    /// <summary>
    /// 配置名称
    /// </summary>
    public static class ConfigurationNames
    {
        /// <summary>
        /// 允许用户编辑
        /// </summary>
        public const string AllowUserToEdit = "AllowUserToEdit";
    }
}
