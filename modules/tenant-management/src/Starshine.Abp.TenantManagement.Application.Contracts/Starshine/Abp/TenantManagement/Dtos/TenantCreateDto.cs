using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// 租户创建
/// </summary>
public class TenantCreateDto : TenantCreateOrUpdateDtoBase
{
    /// <summary>
    /// 管理员邮箱
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public virtual required string AdminEmailAddress { get; set; }

    /// <summary>
    /// 管理员密码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [DisableAuditing]
    public virtual required string AdminPassword { get; set; }
}
