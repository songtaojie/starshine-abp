using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份会话
/// </summary>
public class IdentitySession : BasicAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 会话 ID
    /// </summary>
    public virtual string SessionId { get; protected set; } = string.Empty;

    /// <summary>
    /// 设备
    /// Web, Mobile ...
    /// </summary>
    public virtual string? Device { get; protected set; }
    /// <summary>
    /// 设备信息
    /// </summary>
    public virtual string? DeviceInfo { get; protected set; }
    /// <summary>
    /// 租户id
    /// </summary>
    public virtual Guid? TenantId { get; protected set; }
    /// <summary>
    /// 用户id
    /// </summary>
    public virtual Guid UserId { get; protected set; }
    /// <summary>
    /// 客户端id
    /// </summary>
    public virtual string ClientId { get; set; } = string.Empty;
    /// <summary>
    /// ip地址
    /// </summary>
    public virtual string? IpAddresses { get; protected set; }
    /// <summary>
    /// 登录时间
    /// </summary>
    public virtual DateTime SignedIn { get; protected set; }
    /// <summary>
    /// 最后访问时间
    /// </summary>
    public virtual DateTime? LastAccessed { get; protected set; }
    /// <summary>
    /// 
    /// </summary>
    protected IdentitySession()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sessionId"></param>
    /// <param name="device"></param>
    /// <param name="deviceInfo"></param>
    /// <param name="userId"></param>
    /// <param name="tenantId"></param>
    /// <param name="clientId"></param>
    /// <param name="ipAddresses"></param>
    /// <param name="signedIn"></param>
    /// <param name="lastAccessed"></param>
    public IdentitySession(Guid id, string sessionId,string device,string deviceInfo,Guid userId,Guid? tenantId,string clientId,string ipAddresses, DateTime signedIn, DateTime? lastAccessed = null)
    {
        Id = id;
        SessionId = sessionId;
        Device = device;
        DeviceInfo = deviceInfo;
        UserId = userId;
        TenantId = tenantId;
        ClientId = clientId;
        IpAddresses = ipAddresses;
        SignedIn = signedIn;
        LastAccessed = lastAccessed;
    }
    /// <summary>
    /// 设置登录时间
    /// </summary>
    /// <param name="signedIn"></param>
    public void SetSignedInTime(DateTime signedIn)
    {
        SignedIn = signedIn;
    }
    /// <summary>
    /// 更新最后访问时间
    /// </summary>
    /// <param name="lastAccessed"></param>
    public void UpdateLastAccessedTime(DateTime? lastAccessed)
    {
        LastAccessed = lastAccessed;
    }
    /// <summary>
    /// 设置ip地址
    /// </summary>
    /// <param name="ipAddresses"></param>
    public void SetIpAddresses(IEnumerable<string> ipAddresses)
    {
        IpAddresses = JoinAsString(ipAddresses);
    }
    /// <summary>
    /// 获取ip地址
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetIpAddresses()
    {
        return GetArrayFromString(IpAddresses);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private static string? JoinAsString(IEnumerable<string> list)
    {
        var serialized = string.Join(",", list);
        return serialized.IsNullOrWhiteSpace() ? null : serialized;
    }

    private string[] GetArrayFromString(string? str)
    {
        if (string.IsNullOrEmpty(str)) return [];
        return str.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? [];
    }
}
