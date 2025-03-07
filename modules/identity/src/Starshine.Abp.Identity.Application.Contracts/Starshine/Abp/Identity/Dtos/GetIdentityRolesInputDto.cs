using Volo.Abp.Application.Dtos;

namespace Starshine.Abp.Identity.Dtos;
/// <summary>
/// 获取身份角色输入
/// </summary>
public class GetIdentityRolesInputDto : ExtensiblePagedAndSortedResultRequestDto
{
    /// <summary>
    /// 过滤参数
    /// </summary>
    public string? Filter { get; set; }
}
