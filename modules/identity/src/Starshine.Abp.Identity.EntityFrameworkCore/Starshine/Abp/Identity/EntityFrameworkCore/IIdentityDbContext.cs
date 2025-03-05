using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Starshine.Abp.Identity;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// 身份数据库上下文
/// </summary>
[ConnectionStringName(StarshineIdentityDbProperties.ConnectionStringName)]
public interface IIdentityDbContext : IEfCoreDbContext
{
    /// <summary>
    /// 用户
    /// </summary>
    DbSet<IdentityUser> Users { get; }

    /// <summary>
    /// 角色
    /// </summary>
    DbSet<IdentityRole> Roles { get; }

    /// <summary>
    /// 声明
    /// </summary>
    DbSet<IdentityClaimType> ClaimTypes { get; }

    /// <summary>
    /// 组织
    /// </summary>
    DbSet<OrganizationUnit> OrganizationUnits { get; }

    /// <summary>
    /// 安全日志
    /// </summary>
    DbSet<IdentitySecurityLog> SecurityLogs { get; }

    /// <summary>
    /// 关联用户
    /// </summary>
    DbSet<IdentityLinkUser> LinkUsers { get; }

    /// <summary>
    /// 用户委托
    /// </summary>
    DbSet<IdentityUserDelegation> UserDelegations { get; }

    /// <summary>
    /// 会话
    /// </summary>
    DbSet<IdentitySession> Sessions { get; }
}
