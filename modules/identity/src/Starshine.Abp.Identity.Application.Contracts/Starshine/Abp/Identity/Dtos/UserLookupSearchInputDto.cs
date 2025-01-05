using Volo.Abp.Application.Dtos;

namespace Starshine.Abp.Identity;

/// <summary>
/// 
/// </summary>
public class UserLookupSearchInputDto : ExtensiblePagedAndSortedResultRequestDto
{
    /// <summary>
    /// 
    /// </summary>
    public string? Filter { get; set; }
}
