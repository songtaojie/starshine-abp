using Starshine.Abp.Application.Dtos;
using System;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.TenantManagement.Dtos;
/// <summary>
/// 租户
/// </summary>
public class TenantDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
{
    /// <summary>
    /// 租户名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public required string ConcurrencyStamp { get; set; }
}
