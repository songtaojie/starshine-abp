using System.ComponentModel.DataAnnotations;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
public class IdentityUserUpdateRolesDto
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string[] RoleNames { get; set; } = [];
}
