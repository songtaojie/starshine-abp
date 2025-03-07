namespace Starshine.Abp.IdentityServer.Consts;
/// <summary>
/// 身份服务器安全日志操作常量
/// </summary>
public class IdentityServerSecurityLogActionConsts
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
    /// 登录不允许
    /// </summary>
    public static string LoginNotAllowed { get; set; } = "LoginNotAllowed";

    /// <summary>
    /// 登录需要两步验证
    /// </summary>
    public static string LoginRequiresTwoFactor { get; set; } = "LoginRequiresTwoFactor";

    /// <summary>
    /// 登录失败
    /// </summary>
    public static string LoginFailed { get; set; } = "LoginFailed";

    /// <summary>
    /// 登录无效用户名
    /// </summary>
    public static string LoginInvalidUserName { get; set; } = "LoginInvalidUserName";

    /// <summary>
    /// 登录无效密码
    /// </summary>
    public static string LoginInvalidUserNameOrPassword { get; set; } = "LoginInvalidUserNameOrPassword";
}
