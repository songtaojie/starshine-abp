using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity.Dtos;
/// <summary>
/// 身份用户DTO
/// </summary>
public class IdentityUserDto : ExtensibleFullAuditedEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp, IHasEntityVersion
{
    /// <summary>
    /// 租户ID
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 姓氏
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// 邮件
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// 邮箱是否确认
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 手机号是否确认
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 是否锁定
    /// </summary>
    public bool LockoutEnabled { get; set; }

    /// <summary>
    /// 访问失败次数
    /// </summary>
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// 锁定结束时间
    /// </summary>
    public DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    /// 并发标记
    /// </summary>
    public required string ConcurrencyStamp { get; set; }

    /// <summary>
    /// 实体版本
    /// </summary>
    public int EntityVersion { get; set; }

    /// <summary>
    /// 最后密码更改时间
    /// </summary>
    public DateTimeOffset? LastPasswordChangeTime { get; set; }
}
