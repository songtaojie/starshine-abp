namespace Starshine.Abp.Identity;

/// <summary>
/// 身份数据种子结果
/// </summary>
public class IdentityDataSeedResult
{
    /// <summary>
    /// 创建管理员用户
    /// </summary>
    public bool CreatedAdminUser { get; set; }

    /// <summary>
    /// 创建管理员角色
    /// </summary>
    public bool CreatedAdminRole { get; set; }
}
