using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

/// <summary>
///�����û��ĵ�¼����������ṩ����
/// </summary>
public class IdentityUserLogin : Entity, IMultiTenant
{
    /// <summary>
    /// �⻧id
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    ///��ȡ��������˵�¼�������û���������
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// ��ȡ�����õ�¼�ĵ�¼�ṩ�̣����� facebook��google��
    /// </summary>
    public virtual string LoginProvider { get; protected set; } = string.Empty;

    /// <summary>
    /// ��ȡ�����ô˵�¼��Ψһ�ṩ�����ʶ����
    /// </summary>
    public virtual string ProviderKey { get; protected set; } = string.Empty;

    /// <summary>
    /// ��ȡ�����ô˵�¼�� UI ��ʹ�õ��Ѻ����ơ�
    /// </summary>
    public virtual string? ProviderDisplayName { get; protected set; }
    /// <summary>
    /// 
    /// </summary>
    protected IdentityUserLogin()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="loginProvider"></param>
    /// <param name="providerKey"></param>
    /// <param name="providerDisplayName"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityUserLogin(Guid userId, [NotNull] string loginProvider, [NotNull] string providerKey, string? providerDisplayName, Guid? tenantId)
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));

        UserId = userId;
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
        ProviderDisplayName = providerDisplayName;
        TenantId = tenantId;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="login"></param>
    /// <param name="tenantId"></param>
    protected internal IdentityUserLogin(Guid userId, [NotNull] UserLoginInfo login, Guid? tenantId)
        : this(userId, login.LoginProvider, login.ProviderKey, login.ProviderDisplayName, tenantId)
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual UserLoginInfo ToUserLoginInfo()
    {
        return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [UserId, LoginProvider];
    }
}
