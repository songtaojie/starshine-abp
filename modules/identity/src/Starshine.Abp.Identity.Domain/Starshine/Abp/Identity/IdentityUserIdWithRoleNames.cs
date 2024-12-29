using System;
using System.Collections.Generic;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份用户 ID 和角色名称
/// </summary>
public class IdentityUserIdWithRoleNames
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    public IEnumerable<string>? RoleNames { get; set; }
}