using System.ComponentModel.DataAnnotations;

namespace Starshine.Abp.Identity;

public class IdentityUserUpdateRolesDto
{
    [Required]
    public string[] RoleNames { get; set; }
}
