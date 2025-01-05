using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
public class IdentityUserDto : ExtensibleFullAuditedEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp, IHasEntityVersion
{
    /// <summary>
    /// 
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool LockoutEnabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ConcurrencyStamp { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public int EntityVersion { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset? LastPasswordChangeTime { get; set; }
}
