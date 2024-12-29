using System;
using System.Threading.Tasks;

namespace Starshine.Abp.Identity;

/// <summary>
/// 用户角色查找器接口
/// </summary>
public interface IUserRoleFinder
{
    /// <summary>
    /// 获取用户角色名称
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<string[]> GetRoleNamesAsync(Guid userId);
}
