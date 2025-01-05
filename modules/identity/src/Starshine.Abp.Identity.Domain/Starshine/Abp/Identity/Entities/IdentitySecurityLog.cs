using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SecurityLog;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份安全日志
/// </summary>
public class IdentitySecurityLog : AggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 租户id
    /// </summary>
    public Guid? TenantId { get; protected set; }

    /// <summary>
    /// 应用程序名称
    /// </summary>
    public string? ApplicationName { get; protected set; }

    /// <summary>
    /// 身份
    /// </summary>
    public string? Identity { get; protected set; }

    /// <summary>
    /// Action
    /// </summary>
    public string? Action { get; protected set; }

    /// <summary>
    /// 用户id
    /// </summary>
    public Guid? UserId { get; protected set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string? UserName { get; protected set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string? TenantName { get; protected set; }

    /// <summary>
    /// 客户端编号
    /// </summary>
    public string ClientId { get; protected set; } = string.Empty;

    /// <summary>
    /// 相关标识
    /// </summary>
    public string? CorrelationId { get; protected set; }

    /// <summary>
    /// 客户端 IP 地址
    /// </summary>
    public string? ClientIpAddress { get; protected set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    public string? BrowserInfo { get; protected set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    protected IdentitySecurityLog()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="guidGenerator"></param>
    /// <param name="securityLogInfo"></param>
    public IdentitySecurityLog(IGuidGenerator guidGenerator, SecurityLogInfo securityLogInfo): base(guidGenerator.Create())
    {
        TenantId = securityLogInfo.TenantId;
        TenantName = securityLogInfo.TenantName.Truncate(IdentitySecurityLogConsts.MaxTenantNameLength);

        ApplicationName = securityLogInfo.ApplicationName.Truncate(IdentitySecurityLogConsts.MaxApplicationNameLength);
        Identity = securityLogInfo.Identity.Truncate(IdentitySecurityLogConsts.MaxIdentityLength);
        Action = securityLogInfo.Action.Truncate(IdentitySecurityLogConsts.MaxActionLength);

        UserId = securityLogInfo.UserId;
        UserName = securityLogInfo.UserName.Truncate(IdentitySecurityLogConsts.MaxUserNameLength);

        CreationTime = securityLogInfo.CreationTime;

        ClientIpAddress = securityLogInfo.ClientIpAddress.Truncate(IdentitySecurityLogConsts.MaxClientIpAddressLength);
        ClientId = securityLogInfo.ClientId.Truncate(IdentitySecurityLogConsts.MaxClientIdLength) ?? string.Empty;
        CorrelationId = securityLogInfo.CorrelationId.Truncate(IdentitySecurityLogConsts.MaxCorrelationIdLength);
        BrowserInfo = securityLogInfo.BrowserInfo.Truncate(IdentitySecurityLogConsts.MaxBrowserInfoLength);
        foreach (var property in securityLogInfo.ExtraProperties)
        {
            ExtraProperties.Add(property.Key, property.Value);
        }
    }
}
