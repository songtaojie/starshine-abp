using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Starshine.Abp.Identity;

/// <summary>
/// ��ʾ�û�ӵ�е�������
/// </summary>
public class IdentityUserClaim : IdentityClaim
{
    /// <summary>
    /// ��ȡ���������������ص��û���������
    /// </summary>
    public virtual Guid UserId { get; protected set; }
    /// <summary>
    /// 
    /// </summary>
    protected IdentityUserClaim()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="claim"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityUserClaim(Guid id, Guid userId, [NotNull] Claim claim, Guid? tenantId)
        : base(id, claim, tenantId)
    {
        UserId = userId;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <param name="claimType"></param>
    /// <param name="claimValue"></param>
    /// <param name="tenantId"></param>
    public IdentityUserClaim(Guid id, Guid userId, [NotNull] string claimType, string claimValue, Guid? tenantId)
        : base(id, claimType, claimValue, tenantId)
    {
        UserId = userId;
    }
}