using System.Globalization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.SimpleStateChecking;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限定义序列化
/// </summary>
public class PermissionDefinitionSerializer : IPermissionDefinitionSerializer, ITransientDependency
{
    /// <summary>
    /// 状态检查序列化
    /// </summary>
    protected ISimpleStateCheckerSerializer StateCheckerSerializer { get; }

    /// <summary>
    /// guid生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }

    /// <summary>
    /// 本地化
    /// </summary>
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    /// <summary>
    /// 权限定义序列化
    /// </summary>
    /// <param name="guidGenerator"></param>
    /// <param name="stateCheckerSerializer">状态检查序列化</param>
    /// <param name="localizableStringSerializer">本地化</param>
    public PermissionDefinitionSerializer(IGuidGenerator guidGenerator, ISimpleStateCheckerSerializer stateCheckerSerializer, ILocalizableStringSerializer localizableStringSerializer)
    {
        StateCheckerSerializer = stateCheckerSerializer;
        LocalizableStringSerializer = localizableStringSerializer;
        GuidGenerator = guidGenerator;
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="permissionGroups">权限组定义</param>
    /// <returns></returns>
    public async Task<(PermissionGroupDefinitionRecord[], PermissionDefinitionRecord[])> SerializeAsync(IEnumerable<PermissionGroupDefinition> permissionGroups)
    {
        var permissionGroupRecords = new List<PermissionGroupDefinitionRecord>(permissionGroups.Count());
        var permissionRecords = new List<PermissionDefinitionRecord>();

        foreach (var permissionGroup in permissionGroups)
        {
            permissionGroupRecords.Add(await SerializeAsync(permissionGroup));

            foreach (var permission in permissionGroup.GetPermissionsWithChildren())
            {
                permissionRecords.Add(await SerializeAsync(permission, permissionGroup));
            }
        }

        return (permissionGroupRecords.ToArray(), permissionRecords.ToArray());
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="permissionGroup">权限组定义</param>
    /// <returns></returns>
    public Task<PermissionGroupDefinitionRecord> SerializeAsync(PermissionGroupDefinition permissionGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var permissionGroupRecord = new PermissionGroupDefinitionRecord(
                GuidGenerator.Create(),
                permissionGroup.Name,
                LocalizableStringSerializer.Serialize(permissionGroup.DisplayName));

            foreach (var property in permissionGroup.Properties)
            {
                permissionGroupRecord.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(permissionGroupRecord);
        }
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="permission">权限定义</param>
    /// <param name="permissionGroup">权限组定义</param>
    /// <returns>权限定义记录</returns>
    public Task<PermissionDefinitionRecord> SerializeAsync(PermissionDefinition permission, PermissionGroupDefinition? permissionGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var permissionRecord = new PermissionDefinitionRecord(
                GuidGenerator.Create(),
                permissionGroup?.Name,
                permission.Name,
                permission.Parent?.Name,
                LocalizableStringSerializer.Serialize(permission.DisplayName),
                permission.IsEnabled,
                permission.MultiTenancySide,
                SerializeProviders(permission.Providers),
                SerializeStateCheckers(permission.StateCheckers)
            );

            foreach (var property in permission.Properties)
            {
                permissionRecord.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(permissionRecord);
        }
    }

    /// <summary>
    /// 序列化提供程序
    /// </summary>
    /// <param name="providers">提供商</param>
    /// <returns></returns>
    protected virtual string? SerializeProviders(ICollection<string> providers)
    {
        return providers == null || providers.Count == 0 ?  null : providers.JoinAsString(",");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateCheckers"></param>
    /// <returns></returns>
    protected virtual string? SerializeStateCheckers(List<ISimpleStateChecker<PermissionDefinition>> stateCheckers)
    {
        return StateCheckerSerializer.Serialize(stateCheckers);
    }
}