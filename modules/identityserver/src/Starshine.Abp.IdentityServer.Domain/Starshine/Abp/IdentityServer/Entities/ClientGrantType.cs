using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 客户端授权类型
/// </summary>
public class ClientGrantType : Entity
{
    /// <summary>
    /// 客户端Id
    /// </summary>
    public required virtual Guid ClientId { get; set; }

    /// <summary>
    /// 授权类型
    /// </summary>
    public required virtual string GrantType { get; set; } 

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ClientGrantType()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="grantType"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, string grantType)
    {
        return ClientId == clientId && GrantType == grantType;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ClientId, GrantType];
    }
}
