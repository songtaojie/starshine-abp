using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.Identity.Dtos;
/// <summary>
/// 身份角色
/// </summary>
public class IdentityRoleDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// 是否静态
    /// </summary>
    public bool IsStatic { get; set; }

    /// <summary>
    /// 是否公共
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 并发标记
    /// </summary>
    public required string ConcurrencyStamp { get; set; }
}
