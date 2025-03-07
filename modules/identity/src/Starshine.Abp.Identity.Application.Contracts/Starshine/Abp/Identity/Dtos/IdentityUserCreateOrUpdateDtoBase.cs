using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Starshine.Abp.Identity.Dtos;
/// <summary>
/// 认证用户创建或更新DTO基类
/// </summary>
public abstract class IdentityUserCreateOrUpdateDtoBase : ExtensibleObject
{
    /// <summary>
    /// 用户名称
    /// </summary>
    [Required]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
    public required string UserName { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
    public string? Name { get; set; }

    /// <summary>
    /// 姓氏
    /// </summary>
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
    public string? Surname { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    public required string Email { get; set; }

    /// <summary>
    /// 电话号码
    /// </summary>
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 是否活跃
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 已启用锁定
    /// </summary>
    public bool LockoutEnabled { get; set; }

    /// <summary>
    /// 角色名称列表
    /// </summary>
    public string[]? RoleNames { get; set; }

    /// <summary>
    /// 初始化
    /// </summary>
    protected IdentityUserCreateOrUpdateDtoBase() : base(false)
    {

    }
}
