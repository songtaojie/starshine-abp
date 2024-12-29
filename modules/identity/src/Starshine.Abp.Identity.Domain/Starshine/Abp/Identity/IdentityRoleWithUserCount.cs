using System;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份角色与用户计数
/// </summary>
/// <remarks>
/// </remarks>
/// <param name="role"></param>
/// <param name="userCount"></param>
public class IdentityRoleWithUserCount(IdentityRole role, long userCount)
{
    /// <summary>
    /// 角色
    /// </summary>
    public IdentityRole Role { get; set; } = role;

    /// <summary>
    /// 用户数量
    /// </summary>
    public long UserCount { get; set; } = userCount;
}
