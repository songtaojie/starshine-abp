using Microsoft.EntityFrameworkCore;
using Starshine.Abp.EntityFrameworkCore;
using Starshine.Abp.PermissionManagement.Entities;
using Volo.Abp.Data;

namespace Starshine.Abp.PermissionManagement.EntityFrameworkCore;

/// <summary>
/// 权限管理数据库上下文
/// </summary>
[ConnectionStringName(PermissionManagementDbProperties.ConnectionStringName)]
public interface IPermissionManagementDbContext : IEfCoreDbContext
{
    /// <summary>
    /// 权限组定义记录
    /// </summary>
    DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; }

    /// <summary>
    /// 权限定义记录
    /// </summary>
    DbSet<PermissionDefinitionRecord> Permissions { get; }

    /// <summary>
    /// 权限授予
    /// </summary>
    DbSet<PermissionGrant> PermissionGrants { get; }
}
