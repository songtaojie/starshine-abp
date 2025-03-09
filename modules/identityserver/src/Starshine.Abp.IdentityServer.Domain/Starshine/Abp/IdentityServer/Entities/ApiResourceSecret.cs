using Starshine.IdentityServer;

namespace Starshine.Abp.IdentityServer.Entities;

/// <summary>
/// API 资源密钥
/// </summary>
public class ApiResourceSecret : Secret
{
    /// <summary>
    /// API 资源Id
    /// </summary>
    public required virtual Guid ApiResourceId { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ApiResourceSecret()
    {

    }

    /// <summary>
    /// 判断是否匹配
    /// </summary>
    /// <param name="apiResourceId"></param>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid apiResourceId, string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
    {
        return ApiResourceId == apiResourceId && Value == value && Type == type;
    }

    /// <summary>
    /// 获取密钥
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ApiResourceId, Type, Value];
    }
}
