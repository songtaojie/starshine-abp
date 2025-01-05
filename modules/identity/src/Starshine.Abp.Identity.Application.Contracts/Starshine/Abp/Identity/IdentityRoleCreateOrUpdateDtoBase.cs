using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份角色创建或更新 Dto 库
/// </summary>
public class IdentityRoleCreateOrUpdateDtoBase : ExtensibleObject
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(IdentityRoleConsts), nameof(IdentityRoleConsts.MaxNameLength))]
    [Display(Name = "RoleName")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public bool IsDefault { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsPublic { get; set; }
    /// <summary>
    /// 
    /// </summary>
    protected IdentityRoleCreateOrUpdateDtoBase() : base(false)
    {

    }
}
