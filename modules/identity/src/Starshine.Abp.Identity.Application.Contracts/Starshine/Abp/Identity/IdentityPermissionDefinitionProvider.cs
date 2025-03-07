using Volo.Abp.Authorization.Permissions;
using Starshine.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Starshine.Abp.Identity.Consts;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份许可定义提供者
/// </summary>
public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void Define(IPermissionDefinitionContext context)
    {
        var identityGroup = context.AddGroup(IdentityPermissionConsts.GroupName, L("Permission:IdentityManagement"));

        var rolesPermission = identityGroup.AddPermission(IdentityPermissionConsts.Roles.Default, L("Permission:RoleManagement"));
        rolesPermission.AddChild(IdentityPermissionConsts.Roles.Create, L("Permission:Create"));
        rolesPermission.AddChild(IdentityPermissionConsts.Roles.Update, L("Permission:Edit"));
        rolesPermission.AddChild(IdentityPermissionConsts.Roles.Delete, L("Permission:Delete"));
        rolesPermission.AddChild(IdentityPermissionConsts.Roles.ManagePermissions, L("Permission:ChangePermissions"));

        var usersPermission = identityGroup.AddPermission(IdentityPermissionConsts.Users.Default, L("Permission:UserManagement"));
        usersPermission.AddChild(IdentityPermissionConsts.Users.Create, L("Permission:Create"));
        var editPermission = usersPermission.AddChild(IdentityPermissionConsts.Users.Update, L("Permission:Edit"));
        editPermission.AddChild(IdentityPermissionConsts.Users.ManageRoles, L("Permission:ManageRoles"));
        usersPermission.AddChild(IdentityPermissionConsts.Users.Delete, L("Permission:Delete"));
        usersPermission.AddChild(IdentityPermissionConsts.Users.ManagePermissions, L("Permission:ChangePermissions"));

        identityGroup
            .AddPermission(IdentityPermissionConsts.UserLookup.Default, L("Permission:UserLookup"))
            .WithProviders(ClientPermissionValueProvider.ProviderName);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
