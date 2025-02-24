using Volo.Abp.Reflection;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// 权限
/// </summary>
public static class TenantManagementPermissions
{
    /// <summary>
    /// 租户管理权限
    /// </summary>
    public const string GroupName = "TenantManagement";

    /// <summary>
    /// 租户管理权限
    /// </summary>
    public static class Tenants
    {
        /// <summary>
        /// 租户管理权限
        /// </summary>
        public const string Default = GroupName + ".Tenants";
        /// <summary>
        /// 新增租户权限
        /// </summary>
        public const string Create = Default + ".Create";
        /// <summary>
        /// 修改租户权限
        /// </summary>
        public const string Update = Default + ".Update";
        /// <summary>
        /// 删除租户权限
        /// </summary>
        public const string Delete = Default + ".Delete";
        /// <summary>
        /// 管理租户功能权限
        /// </summary>
        public const string ManageFeatures = Default + ".ManageFeatures";
        /// <summary>
        /// 管理租户连接字符串权限
        /// </summary>
        public const string ManageConnectionStrings = Default + ".ManageConnectionStrings";
    }

    /// <summary>
    /// 获取所有权限
    /// </summary>
    /// <returns></returns>
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(TenantManagementPermissions));
    }
}
