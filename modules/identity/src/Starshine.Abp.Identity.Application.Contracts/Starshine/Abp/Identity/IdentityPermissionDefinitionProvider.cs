﻿using Volo.Abp.Authorization.Permissions;
using Starshine.Abp.Identity.Localization;
using Volo.Abp.Localization;

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
        var identityGroup = context.AddGroup(IdentityPermissions.GroupName, L("Permission:IdentityManagement"));

        var rolesPermission = identityGroup.AddPermission(IdentityPermissions.Roles.Default, L("Permission:RoleManagement"));
        rolesPermission.AddChild(IdentityPermissions.Roles.Create, L("Permission:Create"));
        rolesPermission.AddChild(IdentityPermissions.Roles.Update, L("Permission:Edit"));
        rolesPermission.AddChild(IdentityPermissions.Roles.Delete, L("Permission:Delete"));
        rolesPermission.AddChild(IdentityPermissions.Roles.ManagePermissions, L("Permission:ChangePermissions"));

        var usersPermission = identityGroup.AddPermission(IdentityPermissions.Users.Default, L("Permission:UserManagement"));
        usersPermission.AddChild(IdentityPermissions.Users.Create, L("Permission:Create"));
        var editPermission = usersPermission.AddChild(IdentityPermissions.Users.Update, L("Permission:Edit"));
        editPermission.AddChild(IdentityPermissions.Users.ManageRoles, L("Permission:ManageRoles"));
        usersPermission.AddChild(IdentityPermissions.Users.Delete, L("Permission:Delete"));
        usersPermission.AddChild(IdentityPermissions.Users.ManagePermissions, L("Permission:ChangePermissions"));

        identityGroup
            .AddPermission(IdentityPermissions.UserLookup.Default, L("Permission:UserLookup"))
            .WithProviders(ClientPermissionValueProvider.ProviderName);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityResource>(name);
    }
}
