using Volo.Abp.Reflection;

namespace Starshine.Abp.Identity.Consts;
/// <summary>
/// 身份权限
/// </summary>
public static class IdentityPermissionConsts
{
    /// <summary>
    ///组名
    /// </summary>
    public const string GroupName = "StarshineIdentity";

    /// <summary>
    /// 角色
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// 默认
        /// </summary>
        public const string Default = GroupName + ".Roles";
        /// <summary>
        /// 创建
        /// </summary>
        public const string Create = Default + ".Create";
        /// <summary>
        /// 更新
        /// </summary>
        public const string Update = Default + ".Update";
        /// <summary>
        /// 删除
        /// </summary>
        public const string Delete = Default + ".Delete";
        /// <summary>
        /// 管理权限
        /// </summary>
        public const string ManagePermissions = Default + ".ManagePermissions";
    }
    /// <summary>
    /// 用户
    /// </summary>
    public static class Users
    { 
        /// <summary>
        /// 默认
        /// </summary>
        public const string Default = GroupName + ".Users";
        /// <summary>
        /// 创建
        /// </summary>
        public const string Create = Default + ".Create";
        /// <summary>
        /// 更新
        /// </summary>
        public const string Update = Default + ".Update";
        /// <summary>
        /// 删除
        /// </summary>
        public const string Delete = Default + ".Delete";
        /// <summary>
        /// 管理权限
        /// </summary>
        public const string ManagePermissions = Default + ".ManagePermissions";
        /// <summary>
        /// 管理角色
        /// </summary>
        public const string ManageRoles = Update + ".ManageRoles";
    }
    /// <summary>
    /// 用户查找
    /// </summary>
    public static class UserLookup
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Default = GroupName + ".UserLookup";
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityPermissionConsts));
    }
}
