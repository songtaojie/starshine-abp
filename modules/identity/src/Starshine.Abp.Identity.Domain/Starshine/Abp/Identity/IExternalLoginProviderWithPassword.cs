using System.Threading.Tasks;

namespace Starshine.Abp.Identity
{
    /// <summary>
    /// 外部登录提供者密码
    /// </summary>
    public interface IExternalLoginProviderWithPassword
    {
        /// <summary>
        /// 无需密码即可获取用户信息
        /// </summary>
        bool CanObtainUserInfoWithoutPassword { get; }

        /// <summary>
        /// 当用户通过此源进行身份验证但用户尚不存在时，将调用此方法。因此，源应该创建用户并填充属性。
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="providerName">此提供商的名称</param>
        /// <param name="plainPassword">用户的明文密码</param>
        /// <returns>新创建的用户</returns>
        Task<IdentityUser> CreateUserAsync(string userName, string providerName, string plainPassword);

        /// <summary>
        /// 此方法在现有用户通过此源的身份验证后调用。它可用于通过源更新用户的某些属性。
        /// </summary>
        /// <param name="providerName">此提供商的名称</param>
        /// <param name="user">可更新的用户</param>
        /// <param name="plainPassword">用户的明文密码</param>
        Task UpdateUserAsync(IdentityUser user, string providerName, string plainPassword);
    }
}