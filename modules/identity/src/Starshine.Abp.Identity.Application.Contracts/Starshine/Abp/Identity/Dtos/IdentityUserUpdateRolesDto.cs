using System.ComponentModel.DataAnnotations;

namespace Starshine.Abp.Identity.Dtos;
/// <summary>
/// 认证用户更新角色DTO
/// </summary>
public class IdentityUserUpdateRolesDto
{
    /// <summary>
    /// 角色名称
    /// </summary>
    [Required]
    public required string[] RoleNames { get; set; }
}
