using Volo.Abp.Application.Dtos;

namespace Starshine.Abp.Identity;
/// <summary>
/// 获取身份用户输入
/// </summary>
public class GetIdentityUsersInput : ExtensiblePagedAndSortedResultRequestDto
{
    /// <summary>
    /// 
    /// </summary>
    public string? Filter { get; set; }
}
