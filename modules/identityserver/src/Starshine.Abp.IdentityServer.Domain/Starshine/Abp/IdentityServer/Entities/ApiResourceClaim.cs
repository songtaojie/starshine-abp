namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// API 资源声明
/// </summary>
public class ApiResourceClaim : UserClaim
{
    /// <summary>
    /// API 资源标识
    /// </summary>
    public required virtual Guid ApiResourceId { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal ApiResourceClaim()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="apiResourceId"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid apiResourceId, string type)
    {
        return ApiResourceId == apiResourceId && Type == type;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ApiResourceId, Type];
    }
}
