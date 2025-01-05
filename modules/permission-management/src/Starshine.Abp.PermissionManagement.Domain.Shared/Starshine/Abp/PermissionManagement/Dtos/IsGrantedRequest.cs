using System;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 是否授予请求
/// </summary>
public class IsGrantedRequest
{
    /// <summary>
    /// 授权用户id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 权限名字
    /// </summary>
    public string[]? PermissionNames { get; set; }
}
