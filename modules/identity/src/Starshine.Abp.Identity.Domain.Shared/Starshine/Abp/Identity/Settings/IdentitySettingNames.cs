namespace Starshine.Abp.Identity.Settings;

/// <summary>
/// 资源文件设置
/// </summary>
public static class IdentitySettingNames
{
    private const string Prefix = "Starshine.Identity";

    /// <summary>
    /// 密码提示
    /// </summary>
    public static class Password
    {
        private const string PasswordPrefix = Prefix + ".Password";

        /// <summary>
        /// 长度限制
        /// </summary>
        public const string RequiredLength = PasswordPrefix + ".RequiredLength";
        /// <summary>
        /// 必需唯一字符
        /// </summary>
        public const string RequiredUniqueChars = PasswordPrefix + ".RequiredUniqueChars";
        /// <summary>
        /// 要求非字母数字
        /// </summary>
        public const string RequireNonAlphanumeric = PasswordPrefix + ".RequireNonAlphanumeric";
        /// <summary>
        /// 要求小写
        /// </summary>
        public const string RequireLowercase = PasswordPrefix + ".RequireLowercase";
        /// <summary>
        /// 要求大写
        /// </summary>
        public const string RequireUppercase = PasswordPrefix + ".RequireUppercase";
        /// <summary>
        /// 需要数字
        /// </summary>
        public const string RequireDigit = PasswordPrefix + ".RequireDigit";
        /// <summary>
        /// 强制用户定期更改密码
        /// </summary>
        public const string ForceUsersToPeriodicallyChangePassword = PasswordPrefix + ".ForceUsersToPeriodicallyChangePassword";
        /// <summary>
        /// 密码修改期限天数
        /// </summary>
        public const string PasswordChangePeriodDays = PasswordPrefix + ".PasswordChangePeriodDays";
    }

    /// <summary>
    /// 闭锁
    /// </summary>
    public static class Lockout
    {
        private const string LockoutPrefix = Prefix + ".Lockout";

        /// <summary>
        /// 允许新用户
        /// </summary>
        public const string AllowedForNewUsers = LockoutPrefix + ".AllowedForNewUsers";
        /// <summary>
        /// 锁定时长
        /// </summary>
        public const string LockoutDuration = LockoutPrefix + ".LockoutDuration";
        /// <summary>
        /// 最大失败访问尝试次数
        /// </summary>
        public const string MaxFailedAccessAttempts = LockoutPrefix + ".MaxFailedAccessAttempts";
    }

    /// <summary>
    /// 登入
    /// </summary>
    public static class SignIn
    {
        private const string SignInPrefix = Prefix + ".SignIn";

        /// <summary>
        /// 需要确认电子邮件
        /// </summary>
        public const string RequireConfirmedEmail = SignInPrefix + ".RequireConfirmedEmail";
        /// <summary>
        /// 启用电话号码确认
        /// </summary>
        public const string EnablePhoneNumberConfirmation = SignInPrefix + ".EnablePhoneNumberConfirmation";
        /// <summary>
        /// 需要确认电话号码
        /// </summary>
        public const string RequireConfirmedPhoneNumber = SignInPrefix + ".RequireConfirmedPhoneNumber";
    }

    /// <summary>
    /// 用户
    /// </summary>
    public static class User
    {
        private const string UserPrefix = Prefix + ".User";

        /// <summary>
        /// 已启用用户名更新
        /// </summary>
        public const string IsUserNameUpdateEnabled = UserPrefix + ".IsUserNameUpdateEnabled";
        /// <summary>
        /// 是否已启用电子邮件更新
        /// </summary>
        public const string IsEmailUpdateEnabled = UserPrefix + ".IsEmailUpdateEnabled";
    }

    /// <summary>
    /// 组织单位
    /// </summary>
    public static class OrganizationUnit
    {
        private const string OrganizationUnitPrefix = Prefix + ".OrganizationUnit";

        /// <summary>
        /// 最大用户会员数
        /// </summary>
        public const string MaxUserMembershipCount = OrganizationUnitPrefix + ".MaxUserMembershipCount";
    }
}
