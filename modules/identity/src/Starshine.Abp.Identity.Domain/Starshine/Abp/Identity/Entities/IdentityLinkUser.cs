using System;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份链接用户
/// </summary>
public class IdentityLinkUser : BasicAggregateRoot<Guid>
{
    /// <summary>
    /// 源用户 ID
    /// </summary>
    public virtual Guid SourceUserId { get; protected set; }
    /// <summary>
    /// 源租户id
    /// </summary>
    public virtual Guid? SourceTenantId { get; protected set; }
    /// <summary>
    /// 目标用户id
    /// </summary>
    public virtual Guid TargetUserId { get; protected set; }
    /// <summary>
    /// 目标租户id
    /// </summary>
    public virtual Guid? TargetTenantId { get; protected set; }

    /// <summary>
    /// 初始化 <see cref="IdentityLinkUser"/> 的新实例。
    /// </summary>
    protected IdentityLinkUser()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sourceUser"></param>
    /// <param name="targetUser"></param>
    public IdentityLinkUser(Guid id, IdentityLinkUserInfo sourceUser, IdentityLinkUserInfo targetUser)
        : base(id)
    {
        SourceUserId = sourceUser.UserId;
        SourceTenantId = sourceUser.TenantId;

        TargetUserId = targetUser.UserId;
        TargetTenantId = targetUser.TenantId;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sourceUserId"></param>
    /// <param name="sourceTenantId"></param>
    /// <param name="targetUserId"></param>
    /// <param name="targetTenantId"></param>
    public IdentityLinkUser(Guid id, Guid sourceUserId, Guid? sourceTenantId, Guid targetUserId, Guid? targetTenantId)
        : base(id)
    {
        SourceUserId = sourceUserId;
        SourceTenantId = sourceTenantId;

        TargetUserId = targetUserId;
        TargetTenantId = targetTenantId;
    }
}
