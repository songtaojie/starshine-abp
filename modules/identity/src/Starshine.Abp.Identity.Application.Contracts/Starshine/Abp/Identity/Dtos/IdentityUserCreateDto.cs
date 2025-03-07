using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace Starshine.Abp.Identity.Dtos;
/// <summary>
/// 认证用户创建Dto
/// </summary>
public class IdentityUserCreateDto : IdentityUserCreateOrUpdateDtoBase
{
    /// <summary>
    /// 
    /// </summary>
    [DisableAuditing]
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public required string Password { get; set; }
}
