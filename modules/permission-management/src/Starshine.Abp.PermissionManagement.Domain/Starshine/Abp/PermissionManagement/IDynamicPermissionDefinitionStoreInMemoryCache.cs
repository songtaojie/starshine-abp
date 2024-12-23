using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 动态权限定义存储在内存缓存中
/// </summary>
public interface IDynamicPermissionDefinitionStoreInMemoryCache
{
    /// <summary>
    /// 缓存标记
    /// </summary>
    string CacheStamp { get; set; }

    /// <summary>
    /// 同步信号量
    /// </summary>
    SemaphoreSlim SyncSemaphore { get; }

    /// <summary>
    /// 最后检查时间
    /// </summary>
    DateTime? LastCheckTime { get; set; }

    /// <summary>
    /// 填充权限
    /// </summary>
    /// <param name="permissionGroupRecords"></param>
    /// <param name="permissionRecords"></param>
    /// <returns></returns>
    Task FillAsync(List<PermissionGroupDefinitionRecord> permissionGroupRecords,List<PermissionDefinitionRecord> permissionRecords);

    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    PermissionDefinition? GetPermissionOrNull(string name);

    /// <summary>
    /// 获取所有权限
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<PermissionDefinition> GetPermissions();

    /// <summary>
    /// 获取权限组定义
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<PermissionGroupDefinition> GetGroups();
}