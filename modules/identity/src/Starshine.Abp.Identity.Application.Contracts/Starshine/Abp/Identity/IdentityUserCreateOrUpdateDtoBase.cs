using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
public abstract class IdentityUserCreateOrUpdateDtoBase : ExtensibleObject
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
    public string? Surname { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool LockoutEnabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string[]? RoleNames { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected IdentityUserCreateOrUpdateDtoBase() : base(false)
    {

    }
}
