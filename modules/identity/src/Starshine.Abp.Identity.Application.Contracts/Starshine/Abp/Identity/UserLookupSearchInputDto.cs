using Volo.Abp.Application.Dtos;

namespace Starshine.Abp.Identity;

public class UserLookupSearchInputDto : ExtensiblePagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
