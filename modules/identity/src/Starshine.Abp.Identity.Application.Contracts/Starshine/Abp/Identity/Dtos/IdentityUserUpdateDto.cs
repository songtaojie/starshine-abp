using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Starshine.Abp.Identity.Dtos;
/// <summary>
/// 认证用户更新DTO
/// </summary>
public class IdentityUserUpdateDto : IdentityUserCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    /// <summary>
    /// 密码
    /// </summary>
    [DisableAuditing]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
    public required string Password { get; set; }

    /// <summary>
    /// 并发标记
    /// </summary>
    public required string ConcurrencyStamp { get; set; }
}
