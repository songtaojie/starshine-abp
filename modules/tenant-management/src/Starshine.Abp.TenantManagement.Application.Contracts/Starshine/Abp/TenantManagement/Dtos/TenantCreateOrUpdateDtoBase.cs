using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Starshine.Abp.TenantManagement;

/// <summary>
/// 租户创建/更新 DTO 的基类。
/// </summary>
public abstract class TenantCreateOrUpdateDtoBase : ExtensibleObject
{
    /// <summary>
    /// 租户名称。
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(TenantConsts), nameof(TenantConsts.MaxNameLength))]
    [Display(Name = "TenantName")]
    public required string Name { get; set; }

    /// <summary>
    /// 构造函数。
    /// </summary>
    public TenantCreateOrUpdateDtoBase() : base(false)
    {

    }
}
