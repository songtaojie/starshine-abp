using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
public class IdentityUserCreateDto : IdentityUserCreateOrUpdateDtoBase
{
    /// <summary>
    /// 
    /// </summary>
    [DisableAuditing]
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string Password { get; set; } = string.Empty;
}
