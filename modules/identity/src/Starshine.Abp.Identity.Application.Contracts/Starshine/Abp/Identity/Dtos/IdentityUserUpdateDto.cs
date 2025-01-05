using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    /// <summary>
    /// 
    /// </summary>
    [DisableAuditing]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string ConcurrencyStamp { get; set; } = null!;
}
