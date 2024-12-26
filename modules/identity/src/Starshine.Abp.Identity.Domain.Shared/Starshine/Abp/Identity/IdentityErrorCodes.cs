namespace Starshine.Abp.Identity;

public static class IdentityErrorCodes
{
    public const string UserSelfDeletion = "Starshine.Abp.Identity:010001";
    public const string MaxAllowedOuMembership = "Starshine.Abp.Identity:010002";
    public const string ExternalUserPasswordChange = "Starshine.Abp.Identity:010003";
    public const string DuplicateOrganizationUnitDisplayName = "Starshine.Abp.Identity:010004";
    public const string StaticRoleRenaming = "Starshine.Abp.Identity:010005";
    public const string StaticRoleDeletion = "Starshine.Abp.Identity:010006";
    public const string UsersCanNotChangeTwoFactor = "Starshine.Abp.Identity:010007";
    public const string CanNotChangeTwoFactor = "Starshine.Abp.Identity:010008";
    public const string YouCannotDelegateYourself = "Starshine.Abp.Identity:010009";
    public const string ClaimNameExist = "Starshine.Abp.Identity:010021";
}
