using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份用户委托
/// </summary>
public class IdentityUserDelegation : BasicAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 租户id
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }
    /// <summary>
    /// 源用户id
    /// </summary>
    public virtual Guid SourceUserId { get; protected set; }
    /// <summary>
    /// 目标用户id
    /// </summary>
    public virtual Guid TargetUserId { get; protected set; }
    /// <summary>
    /// 开始时间
    /// </summary>
    public virtual DateTime StartTime { get; protected set; }
    /// <summary>
    /// 截止时间
    /// </summary>
    public virtual DateTime EndTime { get; protected set; }

    /// <summary>
    /// Initializes a new instance of <see cref="IdentityUserDelegation"/>.
    /// </summary>
    protected IdentityUserDelegation()
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sourceUserId"></param>
    /// <param name="targetUserId"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="tenantId"></param>
    public IdentityUserDelegation( Guid id, Guid sourceUserId,Guid targetUserId,DateTime startTime, DateTime endTime, Guid? tenantId = null): base(id)
    {
        TenantId = tenantId;
        SourceUserId = sourceUserId;
        TargetUserId = targetUserId;
        StartTime = startTime;
        EndTime = endTime;
    }
}
