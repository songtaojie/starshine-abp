using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 动态权限定义存储在内存缓存中
/// </summary>
public class DynamicPermissionDefinitionStoreInMemoryCache : IDynamicPermissionDefinitionStoreInMemoryCache,ISingletonDependency
{
    /// <summary>
    /// 缓存标记
    /// </summary>
    public string CacheStamp { get; set; } = string.Empty;

    /// <summary>
    /// 权限组定义
    /// </summary>
    protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions { get; }

    /// <summary>
    /// 权限定义
    /// </summary>
    protected IDictionary<string, PermissionDefinition> PermissionDefinitions { get; }

    /// <summary>
    /// 状态检查器序列化器
    /// </summary>
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }

    /// <summary>
    /// 本地化字符串序列化器
    /// </summary>
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    /// <summary>
    /// 同步信号量
    /// </summary>
    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    /// <summary>
    /// 最后检查时间
    /// </summary>
    public DateTime? LastCheckTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateCheckerSerializer"></param>
    /// <param name="localizableStringSerializer"></param>
    public DynamicPermissionDefinitionStoreInMemoryCache(ISimpleStateCheckerSerializer stateCheckerSerializer,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;

        PermissionGroupDefinitions = new Dictionary<string, PermissionGroupDefinition>();
        PermissionDefinitions = new Dictionary<string, PermissionDefinition>();
    }

    /// <summary>
    /// 填充权限
    /// </summary>
    /// <param name="permissionGroupRecords"></param>
    /// <param name="permissionRecords"></param>
    /// <returns></returns>
    public Task FillAsync(List<PermissionGroupDefinitionRecord> permissionGroupRecords, List<PermissionDefinitionRecord> permissionRecords)
    {
        PermissionGroupDefinitions.Clear();
        PermissionDefinitions.Clear();

        var context = new PermissionDefinitionContext(null!);

        foreach (var permissionGroupRecord in permissionGroupRecords)
        {
            var permissionGroup = context.AddGroup(
                permissionGroupRecord.Name,
                LocalizableStringSerializer.Deserialize(permissionGroupRecord.DisplayName!)
            );

            PermissionGroupDefinitions[permissionGroup.Name] = permissionGroup;

            foreach (var property in permissionGroupRecord.ExtraProperties)
            {
                permissionGroup[property.Key] = property.Value;
            }

            var permissionRecordsInThisGroup = permissionRecords
                .Where(p => p.GroupName == permissionGroup.Name);

            foreach (var permissionRecord in permissionRecordsInThisGroup.Where(x => x.ParentName == null))
            {
                AddPermissionRecursively(permissionGroup, permissionRecord, permissionRecords);
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public PermissionDefinition? GetPermissionOrNull(string name)
    {
        return PermissionDefinitions.GetOrDefault(name);
    }

    /// <summary>
    /// 获取所有权限
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<PermissionDefinition> GetPermissions()
    {
        return PermissionDefinitions.Values.ToList();
    }

    /// <summary>
    /// 获取权限组定义
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<PermissionGroupDefinition> GetGroups()
    {
        return PermissionGroupDefinitions.Values.ToList();
    }

    private void AddPermissionRecursively(ICanAddChildPermission permissionContainer,
        PermissionDefinitionRecord permissionRecord,
        List<PermissionDefinitionRecord> allPermissionRecords)
    {
        var permission = permissionContainer.AddPermission(
            permissionRecord.Name,
            LocalizableStringSerializer.Deserialize(permissionRecord.DisplayName!),
            permissionRecord.MultiTenancySide,
            permissionRecord.IsEnabled
        );

        PermissionDefinitions[permission.Name] = permission;

        if (!permissionRecord.Providers.IsNullOrWhiteSpace())
        {
            permission.Providers.AddRange(permissionRecord.Providers.Split(','));
        }

        if (!permissionRecord.StateCheckers.IsNullOrWhiteSpace())
        {
            var checkers = StateCheckerSerializer
                .DeserializeArray(
                    permissionRecord.StateCheckers,
                    permission
                );
            permission.StateCheckers.AddRange(checkers);
        }

        foreach (var property in permissionRecord.ExtraProperties)
        {
            permission[property.Key] = property.Value;
        }

        foreach (var subPermission in allPermissionRecords.Where(p => p.ParentName == permissionRecord.Name))
        {
            AddPermissionRecursively(permission, subPermission, allPermissionRecords);
        }
    }
}