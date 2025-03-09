using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// 用户声明
/// </summary>
public abstract class UserClaim : Entity
{
    /// <summary>
    /// 类型
    /// </summary>
    public required virtual string Type { get; set; }
    /// <summary>
    /// 构造函数
    /// </summary>
    protected UserClaim()
    {

    }
}
