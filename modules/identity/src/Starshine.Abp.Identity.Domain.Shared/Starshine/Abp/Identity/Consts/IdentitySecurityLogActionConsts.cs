namespace Starshine.Abp.Identity;

/// <summary>
/// 身份安全日志操作常量
/// </summary>
public class IdentitySecurityLogActionConsts
{
    /// <summary>
    /// 登录成功
    /// </summary>
    public static string LoginSucceeded { get; set; } = "LoginSucceeded";

    /// <summary>
    /// 登录锁定
    /// </summary>
    public static string LoginLockedout { get; set; } = "LoginLockedout";

    /// <summary>
    /// 不允许登录
    /// </summary>
    public static string LoginNotAllowed { get; set; } = "LoginNotAllowed";

    /// <summary>
    /// 登录需要双重验证
    /// </summary>
    public static string LoginRequiresTwoFactor { get; set; } = "LoginRequiresTwoFactor";

    /// <summary>
    /// 登录失败
    /// </summary>
    public static string LoginFailed { get; set; } = "LoginFailed";

    /// <summary>
    /// 登录用户名无效
    /// </summary>
    public static string LoginInvalidUserName { get; set; } = "LoginInvalidUserName";

    /// <summary>
    /// 登录用户名或密码无效
    /// </summary>
    public static string LoginInvalidUserNameOrPassword { get; set; } = "LoginInvalidUserNameOrPassword";

    /// <summary>
    /// 登出
    /// </summary>
    public static string Logout { get; set; } = "Logout";

    /// <summary>
    /// 修改用户名
    /// </summary>
    public static string ChangeUserName { get; set; } = "ChangeUserName";

    /// <summary>
    /// 更改电子邮箱
    /// </summary>
    public static string ChangeEmail { get; set; } = "ChangeEmail";

    /// <summary>
    /// 更改电话号码
    /// </summary>
    public static string ChangePhoneNumber { get; set; } = "ChangePhoneNumber";

    /// <summary>
    /// 更改密码
    /// </summary>
    public static string ChangePassword { get; set; } = "ChangePassword";

    /// <summary>
    /// 启用双重验证
    /// </summary>
    public static string TwoFactorEnabled { get; set; } = "TwoFactorEnabled";

    /// <summary>
    /// 双因素禁用
    /// </summary>
    public static string TwoFactorDisabled { get; set; } = "TwoFactorDisabled";
}
