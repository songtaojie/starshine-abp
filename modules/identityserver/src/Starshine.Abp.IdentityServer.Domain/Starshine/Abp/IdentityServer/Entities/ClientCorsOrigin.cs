using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;

/// <summary>
/// 客户端 Cors 来源
/// </summary>
public class ClientCorsOrigin : Entity
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public required virtual Guid ClientId { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    public required virtual string Origin { get; set; }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, string uri)
    {
        return ClientId == clientId && Origin == uri;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ClientCorsOrigin()
    {
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ClientId, Origin];
    }
}
