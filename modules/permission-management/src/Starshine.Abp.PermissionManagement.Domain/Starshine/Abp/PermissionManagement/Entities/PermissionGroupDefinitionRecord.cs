using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限定义记录
/// </summary>
public class PermissionGroupDefinitionRecord : BasicAggregateRoot<Guid>, IHasExtraProperties
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 额外特性
    /// </summary>
    public ExtraPropertyDictionary ExtraProperties { get; protected set; } = [];

    /// <summary>
    /// 权限定义记录
    /// </summary>
    public PermissionGroupDefinitionRecord()
    {
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 权限定义记录
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="name">名称</param>
    /// <param name="displayName">显示名称</param>
    public PermissionGroupDefinitionRecord(Guid id,string name,string? displayName): base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), PermissionGroupDefinitionRecordConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), PermissionGroupDefinitionRecordConsts.MaxDisplayNameLength); ;
        this.SetDefaultsForExtraProperties();
    }

    /// <summary>
    /// 有相同的数据
    /// </summary>
    /// <param name="otherRecord"></param>
    /// <returns></returns>
    public bool HasSameData(PermissionGroupDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name || DisplayName != otherRecord.DisplayName)
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
    /// 修补数据
    /// </summary>
    /// <param name="otherRecord"></param>
    public void Patch(PermissionGroupDefinitionRecord otherRecord)
    {
        if (Name != otherRecord.Name)
        {
            Name = otherRecord.Name;
        }

        if (DisplayName != otherRecord.DisplayName)
        {
            DisplayName = otherRecord.DisplayName;
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
