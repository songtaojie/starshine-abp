using Volo.Abp.Application.Dtos;

namespace Starshine.Abp.Identity;
/// <summary>
/// 获取身份角色输入
/// </summary>
public class GetIdentityRolesInput : ExtensiblePagedAndSortedResultRequestDto
{
    /// <summary>
    /// 过滤参数
    /// </summary>
    public string? Filter { get; set; }
}
