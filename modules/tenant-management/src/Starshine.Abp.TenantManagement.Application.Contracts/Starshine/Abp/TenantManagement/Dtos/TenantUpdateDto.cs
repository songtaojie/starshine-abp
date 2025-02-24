using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// 租户更新Dto
/// </summary>
public class TenantUpdateDto : TenantCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    /// <summary>
    /// 版本
    /// </summary>
    public required string ConcurrencyStamp { get; set; }
}
