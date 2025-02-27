using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
/// 代表身份系统中的角色
/// </summary>
public class IdentityRole : AggregateRoot<Guid>, IMultiTenant, IHasEntityVersion
{
    /// <summary>
    /// 租户uid
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    ///获取或设置此角色的名称。
    /// </summary>
    public virtual string Name { get; protected internal set; }

    /// <summary>
    /// 获取或设置此角色的规范化名称。
    /// </summary>
    [DisableAuditing]
    public virtual string NormalizedName { get; protected internal set; } 

    /// <summary>
    /// 此角色中声明的导航属性。
    /// </summary>
    public virtual ICollection<IdentityRoleClaim> Claims { get; protected set; }

    /// <summary>
    /// 默认角色会自动分配给新用户
    /// </summary>
    public virtual bool IsDefault { get; set; }

    /// <summary>
    ///静态角色无法删除/重命名
    /// </summary>
    public virtual bool IsStatic { get; set; }

    /// <summary>
    /// 用户可以看到其他用户的公共角色
    /// </summary>
    public virtual bool IsPublic { get; set; }

    /// <summary>
    ///每当实体发生变化时，版本值就会增加。
    /// </summary>
    public virtual int EntityVersion { get; protected set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="tenantId"></param>
    public IdentityRole(Guid id, string name, Guid? tenantId = null)
    {
        Check.NotNull(name, nameof(name));
        Id = id;
        Name = name;
        TenantId = tenantId;
        NormalizedName = name.ToUpperInvariant();
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        Claims = new Collection<IdentityRoleClaim>();
    }

    /// <summary>
    /// 添加声明
    /// </summary>
    /// <param name="guidGenerator"></param>
    /// <param name="claim"></param>
    public virtual void AddClaim(IGuidGenerator guidGenerator, Claim claim)
    {
        Check.NotNull(guidGenerator, nameof(guidGenerator));
        Check.NotNull(claim, nameof(claim));
        Claims.Add(new IdentityRoleClaim(guidGenerator.Create(), Id, claim, TenantId));
    }

    /// <summary>
    /// 添加声明
    /// </summary>
    /// <param name="guidGenerator"></param>
    /// <param name="claims"></param>
    public virtual void AddClaims(IGuidGenerator guidGenerator, IEnumerable<Claim> claims)
    {
        Check.NotNull(guidGenerator, nameof(guidGenerator));
        Check.NotNull(claims, nameof(claims));
        foreach (var claim in claims)
        {
            AddClaim(guidGenerator, claim);
        }
    }

    /// <summary>
    /// 获取声明
    /// </summary>
    /// <param name="claim"></param>
    /// <returns></returns>
    public virtual IdentityRoleClaim? FindClaim(Claim claim)
    {
        Check.NotNull(claim, nameof(claim));
        return Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
    }

    /// <summary>
    /// 移除声明
    /// </summary>
    /// <param name="claim"></param>
    public virtual void RemoveClaim(Claim claim)
    {
        Check.NotNull(claim, nameof(claim));
        Claims.RemoveAll(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
    }

    /// <summary>
    /// 改变名字
    /// </summary>
    /// <param name="name"></param>
    public virtual void ChangeName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var oldName = Name;
        Name = name;

        AddLocalEvent(new IdentityRoleNameChangedEto
        {
            Name = name,
            Id = Id,
            TenantId = TenantId,
            OldName = oldName
        });

        AddDistributedEvent(new IdentityRoleNameChangedEto
        {
            Id = Id,
            Name = Name,
            OldName = oldName,
            TenantId = TenantId
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{base.ToString()}, Name = {Name}";
    }
}
