using Volo.Abp.Application.Dtos;

namespace Starshine.Abp.Identity.Dtos;

/// <summary>
/// 用户查询Dto
/// </summary>
public class UserLookupSearchInputDto : ExtensiblePagedAndSortedResultRequestDto
{
    /// <summary>
    /// 过滤参数
    /// </summary>
    public string? Filter { get; set; }
}
