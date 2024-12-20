using System;
using System.Threading.Tasks;

namespace Starshine.Abp.Identity;

public interface IUserRoleFinder
{
    [Obsolete("Use GetRoleNamesAsync instead.")]
    Task<string[]> GetRolesAsync(Guid userId);

    Task<string[]> GetRoleNamesAsync(Guid userId);
}
