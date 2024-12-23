using System;
using System.Collections.Generic;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 是否授权响应
/// </summary>
public class IsGrantedResponse
{
    /// <summary>
    /// 用户id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///授权结果
    /// </summary>
    public Dictionary<string, bool>? Permissions { get; set; }
}
