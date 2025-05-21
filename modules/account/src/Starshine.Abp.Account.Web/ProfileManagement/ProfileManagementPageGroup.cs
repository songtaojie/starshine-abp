using System;
using JetBrains.Annotations;

namespace Starshine.Abp.Account.Web.ProfileManagement;

public class ProfileManagementPageGroup
{
    public string Id {  get; set; }

    public string DisplayName { get; set; }

    public Type ComponentType { get; set; }

    public object? Parameter { get; set; }

    public ProfileManagementPageGroup(string id, string displayName, Type componentType, object? parameter = null)
    {
        Id = id;
        DisplayName = displayName;
        ComponentType = componentType;
        Parameter = parameter;
    }
}
