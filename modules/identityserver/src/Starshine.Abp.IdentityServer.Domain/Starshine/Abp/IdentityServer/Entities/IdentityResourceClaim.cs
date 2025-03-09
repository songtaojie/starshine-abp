namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 身份资源声明
/// </summary>
public class IdentityResourceClaim : UserClaim
{
    /// <summary>
    /// 身份资源Id
    /// </summary>
    public required virtual Guid IdentityResourceId { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    protected internal IdentityResourceClaim()
    {

    }

    /// <summary>
    /// 判断是否相等
    /// </summary>
    /// <param name="identityResourceId"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid identityResourceId, string type)
    {
        return IdentityResourceId == identityResourceId && Type == type;
    }

    /// <summary>
    /// 获取主键
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [IdentityResourceId, Type];
    }
}
