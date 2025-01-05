using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.Identity;
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
    /// 
    /// </summary>
    public bool IsDefault { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsStatic { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 并发标记
    /// </summary>
    public string ConcurrencyStamp { get; set; } = null!;
}
