using Volo.Abp.Domain.Entities.Auditing;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 设备流代码
/// </summary>
public class DeviceFlowCodes : CreationAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 设备代码
    /// </summary>
    public required virtual string DeviceCode { get; set; }

    /// <summary>
    /// 用户代码
    /// </summary>
    public required virtual string UserCode { get; set; }

    /// <summary>
    /// 主体代码
    /// </summary>
    public virtual string? SubjectId { get; set; }

    /// <summary>
    /// 会话代码
    /// </summary>
    public virtual string? SessionId { get; set; }

    /// <summary>
    /// 客户端代码
    /// </summary>
    public required virtual string ClientId { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public virtual DateTimeOffset Expiration { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public virtual string? Data { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected DeviceFlowCodes()
    {

    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="id"></param>
    public DeviceFlowCodes(Guid id) : base(id)
    {

    }
}
