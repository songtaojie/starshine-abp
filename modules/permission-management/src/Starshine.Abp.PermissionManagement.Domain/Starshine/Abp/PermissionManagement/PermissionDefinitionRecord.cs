using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限定义记录
/// </summary>
public class PermissionDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    /// <summary>
    /// 组名称
    /// </summary>
    public string GroupName { get; set; } = null!;

    /// <summary>
    /// 权限名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 父级权限名称
    /// </summary>
    public string? ParentName { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 代表多租户应用程序中的各方
    /// </summary>
    public MultiTenancySides MultiTenancySide { get; set; }

    /// <summary>
    ///以逗号分隔的提供商名称列表。
    /// </summary>
    public string? Providers { get; set; }

    /// <summary>
    /// 序列化字符串来存储有关状态检查器的信息。
    /// </summary>
    public string? StateCheckers { get; set; }

    /// <summary>
    /// 额外信息
    /// </summary>
    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public PermissionDefinitionRecord()
    {
        ExtraProperties = [];
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="groupName"></param>
    /// <param name="name"></param>
    /// <param name="parentName"></param>
    /// <param name="displayName"></param>
    /// <param name="isEnabled"></param>
    /// <param name="multiTenancySide"></param>
    /// <param name="providers"></param>
    /// <param name="stateCheckers"></param>
    public PermissionDefinitionRecord(Guid id,
        string? groupName,
        string name,
        string? parentName,
        string? displayName,
        bool isEnabled = true,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both,
        string? providers = null,
        string? stateCheckers = null)
        : base(id)
    {
        GroupName = Check.NotNullOrWhiteSpace(groupName, nameof(groupName), PermissionGroupDefinitionRecordConsts.MaxNameLength);
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), PermissionDefinitionRecordConsts.MaxNameLength);
        ParentName = Check.Length(parentName, nameof(parentName), PermissionDefinitionRecordConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), PermissionDefinitionRecordConsts.MaxDisplayNameLength);
        IsEnabled = isEnabled;
        MultiTenancySide = multiTenancySide;
        Providers = providers;
        StateCheckers = stateCheckers;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="otherRecord"></param>
    /// <returns></returns>
    public bool HasSameData(PermissionDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            return false;
        }

        if (GroupName != otherRecord.GroupName)
        {
            return false;
        }

        if (ParentName != otherRecord.ParentName)
        {
            return false;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            return false;
        }

        if (IsEnabled != otherRecord.IsEnabled)
        {
            return false;
        }

        if (MultiTenancySide != otherRecord.MultiTenancySide)
        {
            return false;
        }

        if (Providers != otherRecord.Providers)
        {
            return false;
        }

        if (StateCheckers != otherRecord.StateCheckers)
        {
            return false;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="otherRecord"></param>
    public void Patch(PermissionDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            Name = otherRecord.Name;
        }

        if (GroupName != otherRecord.GroupName)
        {
            GroupName = otherRecord.GroupName;
        }

        if (ParentName != otherRecord.ParentName)
        {
            ParentName = otherRecord.ParentName;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
        }

        if (IsEnabled != otherRecord.IsEnabled)
        {
            IsEnabled = otherRecord.IsEnabled;
        }

        if (MultiTenancySide != otherRecord.MultiTenancySide)
        {
            MultiTenancySide = otherRecord.MultiTenancySide;
        }

        if (Providers != otherRecord.Providers)
        {
            Providers = otherRecord.Providers;
        }

        if (StateCheckers != otherRecord.StateCheckers)
        {
            StateCheckers = otherRecord.StateCheckers;
        }

        if (!this.HasSameExtraProperties(otherRecord))
        {
            this.ExtraProperties.Clear();

            foreach (var property in otherRecord.ExtraProperties)
            {
                this.ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}
