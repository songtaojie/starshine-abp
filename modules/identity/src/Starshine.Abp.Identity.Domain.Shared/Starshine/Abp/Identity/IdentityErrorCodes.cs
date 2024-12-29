namespace Starshine.Abp.Identity;

/// <summary>
/// 身份错误代码
/// </summary>
public static class IdentityErrorCodes
{
    /// <summary>
    /// 用户自我删除
    /// </summary>
    public const string UserSelfDeletion = "Starshine.Abp.Identity:010001";
    /// <summary>
    /// 最大允许Ou成员资格
    /// </summary>
    public const string MaxAllowedOuMembership = "Starshine.Abp.Identity:010002";
    /// <summary>
    /// 外部用户密码更改
    /// </summary>
    public const string ExternalUserPasswordChange = "Starshine.Abp.Identity:010003";
    /// <summary>
    /// 重复的组织单位显示名称
    /// </summary>
    public const string DuplicateOrganizationUnitDisplayName = "Starshine.Abp.Identity:010004";
    /// <summary>
    /// 静态角色重命名
    /// </summary>
    public const string StaticRoleRenaming = "Starshine.Abp.Identity:010005";
    /// <summary>
    /// 静态角色删除
    /// </summary>
    public const string StaticRoleDeletion = "Starshine.Abp.Identity:010006";
    /// <summary>
    /// 用户不能更改两个因素
    /// </summary>
    public const string UsersCanNotChangeTwoFactor = "Starshine.Abp.Identity:010007";
    /// <summary>
    /// 不能改变两个因素
    /// </summary>
    public const string CanNotChangeTwoFactor = "Starshine.Abp.Identity:010008";
    /// <summary>
    /// 你不能委托自己
    /// </summary>
    public const string YouCannotDelegateYourself = "Starshine.Abp.Identity:010009";
    /// <summary>
    /// 申索名称存在
    /// </summary>
    public const string ClaimNameExist = "Starshine.Abp.Identity:010021";
}
