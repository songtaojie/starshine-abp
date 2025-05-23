﻿using Starshine.Abp.Domain.Entities;
using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.PermissionManagement.Entities;

/// <summary>
/// 权限定义记录
/// </summary>
public class PermissionDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    /// <summary>
    /// 组名称
    /// </summary>
    public required string GroupName { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 父级权限名称
    /// </summary>
    public string? ParentName { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public required string DisplayName { get; set; }

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
    /// 权限定义记录
    /// </summary>
    public PermissionDefinitionRecord()
    {
        GroupName = string.Empty; 
        Name = string.Empty;
        DisplayName = string.Empty;
        ExtraProperties = [];
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 权限定义记录
    /// </summary>
    /// <param name="id"></param>
    public PermissionDefinitionRecord(Guid id)
        : base(id)
    {
        ExtraProperties = [];
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 有相同的数据
    /// </summary>
    /// <param name="otherRecord">其他记录</param>
    /// <returns></returns>
    public bool HasSameData(PermissionDefinitionRecord otherRecord)
    {
        if(otherRecord == null) return false;
        var propertiesToCompare = new (object? Current, object? Other)[]
        {
            (Name, otherRecord.Name),
            (GroupName, otherRecord.GroupName),
            (ParentName, otherRecord.ParentName),
            (DisplayName, otherRecord.DisplayName),
            (IsEnabled, otherRecord.IsEnabled),
            (MultiTenancySide, otherRecord.MultiTenancySide),
            (Providers, otherRecord.Providers),
            (StateCheckers, otherRecord.StateCheckers)
        };
        // 检查是否所有属性都相等
        if (propertiesToCompare.Any(pair => !Equals(pair.Current, pair.Other)))
        {
            return false;
        }
      
        return this.HasSameExtraProperties(otherRecord);
    }

    /// <summary>
    /// 修正数据
    /// </summary>
    /// <param name="otherRecord">其他记录</param>
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
            ExtraProperties.Clear();

            foreach (var property in otherRecord.ExtraProperties)
            {
                ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}
