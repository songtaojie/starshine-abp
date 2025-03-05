using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 代表组织单位 (OU)。
/// </summary>
public class OrganizationUnit : FullAuditedAggregateRoot<Guid>, IMultiTenant, IHasEntityVersion
{
    /// <summary>
    /// 租户uid
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// 父级 <see cref="OrganizationUnit"/> Id。如果此 OU 是根，则为 Null。
    /// </summary>
    public virtual Guid? ParentId { get; internal set; }

    /// <summary>
    /// 此组织单位的层级代码。示例：“00001.00042.00005”。这是组织单位的唯一代码。如果 OU 层级发生变化，该代码也会发生变化。
    /// </summary>
    public virtual string Code { get; internal set; } = string.Empty;

    /// <summary>
    /// 此 OrganizationUnit 的显示名称。
    /// </summary>
    public virtual string? DisplayName { get; set; }

    /// <summary>
    ///每当实体发生变化时，版本值就会增加。
    /// </summary>
    public virtual int EntityVersion { get; set; }

    /// <summary>
    ///此 OU 的角色。
    /// </summary>
    public virtual ICollection<OrganizationUnitRole> Roles { get; protected set; } = [];

    /// <summary>
    ///  初始化 <see cref="OrganizationUnit"/> 类的新实例。
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="displayName">显示名称。</param>
    /// <param name="parentId">如果 OU 是根，则为父级的 ID 或空。</param>
    /// <param name="tenantId">租户 ID 或null。</param>
    public OrganizationUnit(Guid id, string displayName, Guid? parentId = null, Guid? tenantId = null)
        : base(id)
    {
        TenantId = tenantId;
        DisplayName = displayName;
        ParentId = parentId;
        Roles = new Collection<OrganizationUnitRole>();
    }

    /// <summary>
    /// 为给定的数字创建代码。例如：如果数字为 4,2，则返回“00004.00002”；
    /// </summary>
    /// <param name="numbers">Numbers</param>
    public static string? CreateCode(params int[] numbers)
    {
        if (numbers.IsNullOrEmpty())
        {
            return null;
        }

        return numbers.Select(number => number.ToString(new string('0', OrganizationUnitConsts.CodeUnitLength))).JoinAsString(".");
    }

    /// <summary>
    /// 将子代码附加到父代码。例如：如果 parentCode =“00001”，childCode =“00042”，则返回“00001.00042”。
    /// </summary>
    /// <param name="parentCode">父代码。如果父级是根，则可以为 null 或为空。</param>
    /// <param name="childCode">子代码。</param>
    public static string AppendCode(string? parentCode, string? childCode)
    {
        if (childCode.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return childCode;
        }

        return parentCode + "." + childCode;
    }

    /// <summary>
    /// 获取相对于父级的代码。例如：如果 code = “00019.00055.00001” 且 parentCode = “00019”，则返回“00055.00001”。
    /// </summary>
    /// <param name="code">代码。</param>
    /// <param name="parentCode">父代码。</param>
    public static string? GetRelativeCode(string code, string parentCode)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "代码不能为空。");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return code;
        }

        if (code.Length == parentCode.Length)
        {
            return null;
        }

        return code.Substring(parentCode.Length + 1);
    }

    /// <summary>
    ///计算给定代码的下一个代码。例如：如果 code = “00019.00055.00001”，则返回“00019.00055.00002”。
    /// </summary>
    /// <param name="code">代码。</param>
    public static string CalculateNextCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var parentCode = GetParentCode(code);
        var lastUnitCode = GetLastUnitCode(code);

        return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
    }

    /// <summary>
    /// 获取最后一个单位代码。例如：如果 code = “00019.00055.00001”，则返回“00001”。
    /// </summary>
    /// <param name="code">The code.</param>
    public static string GetLastUnitCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        return splittedCode[splittedCode.Length - 1];
    }

    /// <summary>
    /// 获取父代码。例如：如果 code = “00019.00055.00001”，则返回“00019.00055”。
    /// </summary>
    /// <param name="code">The code.</param>
    public static string? GetParentCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        if (splittedCode.Length == 1)
        {
            return null;
        }

        return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
    }

    /// <summary>
    /// 添加角色信息
    /// </summary>
    /// <param name="roleId"></param>
    public virtual void AddRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (IsInRole(roleId))
        {
            return;
        }

        Roles.Add(new OrganizationUnitRole(roleId, Id, TenantId));
    }

    /// <summary>
    /// 移除角色信息
    /// </summary>
    /// <param name="roleId"></param>
    public virtual void RemoveRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (!IsInRole(roleId))
        {
            return;
        }

        Roles.RemoveAll(r => r.RoleId == roleId);
    }

    /// <summary>
    /// 是否存在角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public virtual bool IsInRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        return Roles.Any(r => r.RoleId == roleId);
    }
}
