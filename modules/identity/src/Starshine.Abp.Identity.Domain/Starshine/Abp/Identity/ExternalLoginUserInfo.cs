using JetBrains.Annotations;
using Volo.Abp;

namespace Starshine.Abp.Identity;

/// <summary>
/// 外部登录用户信息
/// </summary>
public class ExternalLoginUserInfo
{
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 姓
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// 电话号码
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// 电话号码已确认
    /// </summary>
    public bool? PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// 电子邮件确认
    /// </summary>
    public bool? EmailConfirmed { get; set; }

    /// <summary>
    /// 启用双重验证
    /// </summary>
    public bool? TwoFactorEnabled { get; set; }

    /// <summary>
    /// 提供者密钥
    /// </summary>
    public string ProviderKey { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    public ExternalLoginUserInfo([NotNull] string email)
    {
        Email = Check.NotNullOrWhiteSpace(email, nameof(email));
    }
}
