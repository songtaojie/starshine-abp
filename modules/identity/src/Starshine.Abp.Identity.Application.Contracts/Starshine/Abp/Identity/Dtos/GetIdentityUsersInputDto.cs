using Volo.Abp.Application.Dtos;

namespace Starshine.Abp.Identity.Dtos;
/// <summary>
/// 获取身份用户输入
/// </summary>
public class GetIdentityUsersInputDto : ExtensiblePagedAndSortedResultRequestDto
{
    /// <summary>
    /// 
    /// </summary>
    public string? Filter { get; set; }
}
