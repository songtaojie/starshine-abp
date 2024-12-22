using JetBrains.Annotations;

namespace Starshine.Abp.Users;

/// <summary>
/// 更新用户
/// </summary>
public interface IUpdateUserData
{
    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    bool Update([NotNull] IUserData user);
}
